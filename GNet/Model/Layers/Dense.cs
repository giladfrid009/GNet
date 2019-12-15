using GNet.Extensions.Array;
using GNet.Extensions.IArray;
using GNet.Extensions.ShapedArray;
using System;

namespace GNet
{
    // todo: implement iarray? or ishapedArray ?
    [Serializable]
    public class Dense : ICloneable<Dense>
    {
        public ShapedArray<Neuron> Neurons { get; }
        public IActivation Activation { get; }
        public IInitializer WeightInit { get; }
        public IInitializer BiasInit { get; }
        public Shape Shape { get; }

        public Dense(Shape shape, IActivation activation, IInitializer weightInit, IInitializer biasInit)
        {
            Shape = shape.Clone();
            Activation = activation.Clone();
            WeightInit = weightInit.Clone();
            BiasInit = biasInit.Clone();

            Neurons = new ShapedArray<Neuron>(Shape, new int[Shape.Length()].Select(X => new Neuron()));
        }

        public virtual void Connect(Dense inLayer)
        {
            inLayer.Neurons.ForEach(N => N.OutSynapses = new ShapedArrayMutable<Synapse>(Shape));
            Neurons.ForEach(N => N.InSynapses = new ShapedArrayMutable<Synapse>(inLayer.Shape));

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
            int outLength = Shape.Length();

            Neurons.ForEach(N =>
            {
                N.Bias = BiasInit.Initialize(inLength, outLength);
                N.InSynapses.ForEach(W => W.Weight = WeightInit.Initialize(inLength, outLength));
            });
        }

        public void SetInputs(ShapedArray<double> values)
        {
            if (values.Shape.Equals(Shape) == false)
            {
                throw new ArgumentOutOfRangeException("values length mismatch.");
            }

            Neurons.ForEach((N, i) => N.ActivatedValue = values[i]);
        }

        public void FeedForward()
        {
            Neurons.ForEach(N => N.Value = N.Bias + N.InSynapses.Sum(W => W.Weight * W.InNeuron.ActivatedValue));

            ShapedArray<double> activated = Activation.Activate(Neurons.Select(N => N.Value));

            Neurons.ForEach((N, i) => N.ActivatedValue = activated[i]);
        }

        public void CalcGrads(ILoss loss, ShapedArray<double> targets)
        {
            if (targets.Shape.Equals(Shape) == false)
            {
                throw new ArgumentException("targets shape mismatch.");
            }

            if (loss is IOutTransformer)
            {
                throw new ArgumentException($"{nameof(loss)} loss doesn't support backpropogation.");
            }

            ShapedArray<double> actvDers = Activation.Derivative(Neurons.Select(N => N.Value));
            ShapedArray<double> lossDers = loss.Derivative(targets, Neurons.Select(N => N.ActivatedValue));
            ShapedArray<double> grads = lossDers.Combine(actvDers, (LD, AD) => LD * AD);

            Neurons.ForEach((N, i) =>
            {
                N.Gradient = grads[i];
                N.InSynapses.ForEach(S => S.Gradient = N.Gradient * S.InNeuron.ActivatedValue);
            });
        }

        public void CalcGrads()
        {
            ShapedArray<double> actvDers = Activation.Derivative(Neurons.Select(N => N.Value));

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
