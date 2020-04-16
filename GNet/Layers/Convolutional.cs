using System;
using System.Collections.Generic;
using GNet.Utils.Convolutional;
using GNet.Model;
using GNet.Model.Convolutional;

namespace GNet.Layers
{
    [Serializable]
    public class Convolutional : TrainableLayer, IConvLayer
    {
        public override ImmutableShapedArray<Neuron> Neurons { get; }
        public ImmutableArray<Kernel> Kernels { get; }
        public ImmutableArray<int> Strides { get; }
        public ImmutableArray<int> Paddings { get; }
        public override Shape Shape { get; }
        public Shape InputShape { get; }
        public Shape PaddedShape { get; }
        public Shape KernelShape { get; }
        public int KernelsNum { get; }

        public Convolutional(Shape inputShape, Shape kernelShape, int nKernels, ImmutableArray<int> strides, ImmutableArray<int> paddings, IActivation activation, IInitializer weightInit, IInitializer biasInit) :
            base(activation, biasInit, weightInit)
        {
            Validator.CheckParams(inputShape, kernelShape, strides, paddings);

            if (nKernels < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(nKernels));
            }

            InputShape = inputShape;
            KernelShape = kernelShape;
            Strides = strides;
            Paddings = paddings;
            KernelsNum = nKernels;

            PaddedShape = Pad.Shape(inputShape, paddings);

            Kernels = new ImmutableArray<Kernel>(nKernels, () => new Kernel(kernelShape));

            Shape = CalcOutShape(inputShape, kernelShape, nKernels, strides, paddings);

            Neurons = new ImmutableShapedArray<Neuron>(Shape, () => new CNeuron());
        }

        private static Shape CalcOutShape(Shape inputShape, Shape kernelShape, int nKernels, ImmutableArray<int> strides, ImmutableArray<int> paddings)
        {
            ImmutableArray<int> channelDims = inputShape.Dimensions.Select((D, i) => 1 + (D + 2 * paddings[i] - kernelShape.Dimensions[i]) / strides[i]);

            return new Shape(new ImmutableArray<int>(nKernels).Concat(channelDims));
        }

        public override void Connect(ILayer inLayer)
        {
            if (inLayer.Shape != InputShape)
            {
                throw new ShapeMismatchException(nameof(inLayer));
            }

            ImmutableShapedArray<Neuron> padded = Pad.ShapedArray(inLayer.Neurons, Paddings, () => new Neuron());

            var inConnections = new ImmutableShapedArray<List<Synapse>>(PaddedShape, () => new List<Synapse>());

            Kernels.ForEach((kernel, i) =>
            {
                int offset = i * Shape.Volume / KernelsNum;

                IndexGen.ByStrides(PaddedShape, Strides, KernelShape).ForEach((idxKernel, j) =>
                {
                    var N = (CNeuron)Neurons[offset + j];

                    N.KernelBias = kernel.Bias;

                    N.InSynapses = IndexGen.ByStart(KernelShape, ImmutableArray<int>.FromRef(idxKernel)).Select((idx, k) =>
                    {
                        var S = new CSynapse(padded[idx], N)
                        {
                            KernelWeight = kernel.Weights[k]
                        };

                        inConnections[idx].Add(S);
                        return (Synapse)S;
                    });
                });
            });

            padded.ForEach((N, i) => N.OutSynapses = new ImmutableArray<Synapse>(inConnections[i]));
        }

        public override void Initialize()
        {
            int inLength = KernelShape.Volume;

            Kernels.ForEach(K =>
            {
                K.Bias.Value = BiasInit.Initialize(inLength, 1);
                K.Weights.ForEach(W => W.Value = WeightInit.Initialize(inLength, 1));
            });
        }

        public override void Input(ImmutableShapedArray<double> values)
        {
            throw new NotSupportedException();
        }
    }
}