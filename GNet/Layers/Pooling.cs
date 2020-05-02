using System;
using System.Collections.Generic;
using GNet.Model;
using GNet.Utils.Convolutional;

namespace GNet.Layers
{
    [Serializable]
    public class Pooling : IConvLayer
    {
        public ImmutableShapedArray<Neuron> Neurons { get; }
        public ImmutableArray<int> Strides { get; }
        public ImmutableArray<int> Paddings { get; }
        public Shape InputShape { get; }
        public Shape PaddedShape { get; }
        public Shape KernelShape { get; }
        public Shape Shape { get; }
        public IPooler Pooler { get; }

        public Pooling(Shape inputShape, Shape outputShape, Shape kernelShape, ImmutableArray<int> strides, IPooler pooler)
        {
            InputShape = inputShape;
            Shape = outputShape;
            KernelShape = kernelShape;
            Strides = strides;
            Pooler = pooler;

            Paddings = Padder.CalcPadding(inputShape, outputShape, kernelShape, strides, true);

            PaddedShape = Padder.PadShape(inputShape, Paddings);

            Neurons = new ImmutableShapedArray<Neuron>(outputShape, () => new Neuron());
        }

        public void Connect(ILayer inLayer)
        {
            if (inLayer.Shape != InputShape)
            {
                throw new ShapeMismatchException(nameof(inLayer));
            }

            ImmutableShapedArray<Neuron> padded = Padder.PadShapedArray(inLayer.Neurons, Paddings, () => new Neuron());

            var inConnections = new ImmutableShapedArray<List<Synapse>>(PaddedShape, () => new List<Synapse>());

            IndexGen.ByStrides(PaddedShape, Strides, KernelShape).ForEach((idxKernel, i) =>
            {
                Neuron N = Neurons[i];

                N.InSynapses = IndexGen.ByStart(KernelShape, ImmutableArray<int>.FromRef(idxKernel)).Select((idx, j) =>
                {
                    var S = new Synapse(padded[idx], N);
                    inConnections[idx].Add(S);
                    return S;
                });
            });

            padded.ForEach((N, i) => N.OutSynapses = new ImmutableArray<Synapse>(inConnections[i].ToArray()));
        }

        public void Initialize()
        {
        }

        public void Input(ImmutableShapedArray<double> values, bool isTraining)
        {
            throw new NotSupportedException();
        }

        public void Forward(bool isTraining)
        {
            Neurons.ForEach(N =>
            {
                ImmutableArray<double> inWeights = Pooler.CalcWeights(N.InSynapses);

                N.InSynapses.ForEach((S, i) => S.Weight = inWeights[i]);

                N.InVal = N.InSynapses.Sum(S => S.Weight * S.InNeuron.OutVal);

                N.OutVal = N.InVal;
            });
        }

        public void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            Neurons.ForEach((N, i) => N.Gradient = loss.Derivative(targets[i], N.OutVal));
        }

        public void CalcGrads()
        {
            Neurons.ForEach((N, i) => N.Gradient = N.OutSynapses.Sum(S => S.Weight * S.OutNeuron.Gradient));
        }

        public void Optimize(IOptimizer optimizer)
        {
        }

        public void Update()
        {
        }       
    }
}