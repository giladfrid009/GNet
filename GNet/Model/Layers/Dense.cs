﻿using GNet.Extensions.IArray;
using GNet.Extensions.IShapedArray;
using System;

namespace GNet
{
    [Serializable]
    public class Dense : ICloneable<Dense>
    {
        public ShapedArrayImmutable<Neuron> Neurons { get; }
        public IActivation Activation { get; }
        public IInitializer WeightInit { get; }
        public IInitializer BiasInit { get; }
        public Shape Shape { get; }

        public Dense(Shape shape, IActivation activation, IInitializer weightInit, IInitializer biasInit)
        {
            Shape = shape;
            Activation = activation.Clone();
            WeightInit = weightInit.Clone();
            BiasInit = biasInit.Clone();

            Neurons = new ShapedArrayImmutable<Neuron>(Shape, () => new Neuron());
        }

        public virtual void Connect(Dense inLayer)
        {
            inLayer.Neurons.ForEach(N => N.OutSynapses = new ShapedArray<Synapse>(Shape));
            Neurons.ForEach(N => N.InSynapses = new ShapedArray<Synapse>(inLayer.Shape));

            Neurons.ForEach((outN, i) =>
            {
                inLayer.Neurons.ForEach((inN, j) =>
                {
                    Synapse W = new Synapse(inN, outN);
                    inN.OutSynapses[i] = W;
                    outN.InSynapses[j] = W;
                });
            });
        }

        public void Initialize()
        {
            int inLength = Neurons[0].InSynapses.Length;
            int outLength = Shape.Volume;

            Neurons.ForEach(N =>
            {
                N.Bias = BiasInit.Initialize(inLength, outLength);
                N.InSynapses.ForEach(W => W.Weight = WeightInit.Initialize(inLength, outLength));
            });
        }

        public void SetInputs(ShapedArrayImmutable<double> values)
        {
            if (values.Shape != Shape)
            {
                throw new ArgumentOutOfRangeException("values length mismatch.");
            }

            Neurons.ForEach((N, i) => N.ActivatedValue = values[i]);
        }

        public void FeedForward()
        {
            Neurons.ForEach(N => N.Value = N.Bias + N.InSynapses.Sum(W => W.Weight * W.InNeuron.ActivatedValue));

            ShapedArrayImmutable<double> activated = Activation.Activate(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }

        public void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
        {
            if (targets.Shape.Equals(Shape) == false)
            {
                throw new ArgumentException("targets shape mismatch.");
            }

            if (loss is IOutTransformer)
            {
                throw new ArgumentException($"{nameof(loss)} loss doesn't support backpropogation.");
            }

            ShapedArrayImmutable<double> actvDers = Activation.Derivative(Neurons.Select(N => N.Value));
            ShapedArrayImmutable<double> lossDers = loss.Derivative(targets, Neurons.Select(N => N.ActivatedValue));
            ShapedArrayImmutable<double> grads = lossDers.Combine(actvDers, (LD, AD) => LD * AD);

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = grads[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public void CalcGrads()
        {
            ShapedArrayImmutable<double> actvDers = Activation.Derivative(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Sum(W => W.Weight * W.OutNeuron.Gradient) * actvDers[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public virtual void Update()
        {
            Neurons.ForEach(N =>
            {
                N.Bias += N.BatchBias;
                N.BatchBias = 0.0;

                N.InSynapses.ForEach(S =>
                {
                    S.Weight += S.BatchWeight;
                    S.BatchWeight = 0.0;
                });
            });
        }

        public virtual Dense Clone()
        {
            return new Dense(Shape, Activation, WeightInit, BiasInit);
        }
    }
}
