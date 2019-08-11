using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using System;

namespace GNet
{
    [Serializable]
    public class Dense : ICloneable<Dense>
    {
        public Neuron[] Neurons { get; } = new Neuron[0];
        public IActivation Activation { get; }
        public IInitializer WeightInit { get; }
        public IInitializer BiasInit { get; }
        public int Length { get; }

        public Dense(int length, IActivation activation, IInitializer weightInit, IInitializer biasInit)
        {
            Length = length;
            Activation = activation.Clone();
            WeightInit = weightInit.Clone();
            BiasInit = biasInit.Clone();

            Neurons = new Neuron[Length];

            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new Neuron();
            }
        }

        public virtual void Connect(Dense inLayer)
        {
            inLayer.Neurons.ForEach(N => N.OutSynapses = new Synapse[Length]);
            Neurons.ForEach(N => N.InSynapses = new Synapse[inLayer.Length]);

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

            Neurons.ForEach(N =>
            {
                N.Bias = BiasInit.Initialize(inLength, Length);
                N.InSynapses.ForEach(W => W.Weight = WeightInit.Initialize(inLength, Length));
            });
        }

        public void SetInputs(double[] values)
        {
            if (values.Length != Length)
            {
                throw new ArgumentOutOfRangeException("Values length mismatch.");
            }

            Neurons.ForEach((N, i) => N.ActivatedValue = values[i]);
        }

        public void FeedForward()
        {
            Neurons.ForEach(N => N.Value = N.Bias + N.InSynapses.Accumulate(0.0, (R, W) => R + W.Weight * W.InNeuron.ActivatedValue));

            double[] activated = Activation.Activate(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }

        public void CalcGrads(ILoss loss, double[] targets)
        {
            if (loss is IOutTransformer)
            {
                throw new ArgumentException("This loss doesn't support backpropogation.");
            }

            double[] actvDers = Activation.Derivative(Neurons.Select(N => N.Value));
            double[] lossDers = loss.Derivative(targets, Neurons.Select(N => N.ActivatedValue));
            double[] grads = lossDers.Combine(actvDers, (LD, AD) => LD * AD);

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = grads[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public void CalcGrads()
        {
            double[] actvDers = Activation.Derivative(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = N.OutSynapses.Accumulate(0.0, (R, W) => R + W.Weight * W.OutNeuron.Gradient) * actvDers[i];
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
            return new Dense(Length, Activation, WeightInit, BiasInit);
        }
    }
}
