using System;
using System.Collections.Generic;
using GNet.Model;
using GNet.Utils.Convolutional;

namespace GNet.Layers
{
    [Serializable]
    public class Pooling : IConvLayer
    {
        public ShapedArrayImmutable<Neuron> Neurons { get; }
        public ArrayImmutable<int> Strides { get; }
        public ArrayImmutable<int> Paddings { get; }
        public Shape InputShape { get; }
        public Shape PaddedShape { get; }
        public Shape KernelShape { get; }
        public Shape Shape { get; }
        public IPooler Pooler { get; }
        public bool IsTrainable { get; } = false;

        public Pooling(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, IPooler pooler)
        {
            ConvValidator.CheckParams(inputShape, kernelShape, strides, paddings);

            Pooler = pooler;
            InputShape = inputShape;
            KernelShape = kernelShape;
            Strides = strides;
            Paddings = paddings;

            PaddedShape = Pad.Shape(inputShape, paddings);

            Shape = CalcOutShape(inputShape, kernelShape, strides, paddings);

            Neurons = new ShapedArrayImmutable<Neuron>(Shape, () => new Neuron());
        }

        private static Shape CalcOutShape(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings)
        {
            return new Shape(inputShape.Dimensions.Select((D, i) => 1 + (D + 2 * paddings[i] - kernelShape.Dimensions[i]) / strides[i]));
        }

        public void Connect(ILayer inLayer)
        {
            if (inLayer.Shape != InputShape)
            {
                throw new ShapeMismatchException(nameof(inLayer));
            }

            ShapedArrayImmutable<Neuron> padded = Pad.ShapedArray(inLayer.Neurons, Paddings, () => new Neuron());

            var inConnections = new ShapedArrayImmutable<List<Synapse>>(PaddedShape, () => new List<Synapse>());

            IndexGen.ByStrides(PaddedShape, Strides, KernelShape).ForEach((idxKernel, i) =>
            {
                Neuron N = Neurons[i];

                N.InSynapses = IndexGen.ByStart(KernelShape, new ArrayImmutable<int>(idxKernel)).Select((idx, j) =>
                {
                    var S = new Synapse(padded[idx], N);
                    inConnections[idx].Add(S);
                    return S;
                });
            });

            padded.ForEach((N, i) => N.OutSynapses = new ArrayImmutable<Synapse>(inConnections[i]));
        }

        public void Initialize()
        {
        }

        public void Input(ShapedArrayImmutable<double> values)
        {
            throw new NotSupportedException();
        }

        public void Forward()
        {
            Neurons.ForEach(N =>
            {
                N.Value = Pooler.Pool(N.InSynapses.Select(S => S.InNeuron.ActivatedValue), out ArrayImmutable<double> inWeights);

                N.ActivatedValue = N.Value;

                N.InSynapses.ForEach((S, i) => S.Weight = inWeights[i]);
            });
        }

        public void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
        {
            if (targets.Shape != Shape)
            {
                throw new ShapeMismatchException(nameof(targets));
            }

            ShapedArrayImmutable<double> grads = loss.Derivative(targets, Neurons.Select(N => N.ActivatedValue));

            Neurons.ForEach((N, i) => N.Gradient = grads[i]);
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