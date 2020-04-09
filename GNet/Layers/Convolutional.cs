using System;
using System.Collections.Generic;
using GNet.Utils.Convolutional;
using GNet.Model;
using GNet.Model.Convolutional;

namespace GNet.Layers
{
    [Serializable]
    public class Convolutional : TrainableLayer<CNeuron>, IConvLayer
    {
        public ArrayImmutable<Kernel> Kernels { get; }
        public ArrayImmutable<int> Strides { get; }
        public ArrayImmutable<int> Paddings { get; }
        public Shape InputShape { get; }
        public Shape PaddedShape { get; }
        public Shape KernelShape { get; }
        public int KernelsNum => Kernels.Length;

        public Convolutional(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, int nKernels, IActivation activation, IInitializer weightInit, IInitializer biasInit) :
            base(CalcOutShape(inputShape, kernelShape, strides, paddings, nKernels), activation, biasInit, weightInit)
        {
            InputShape = inputShape;
            KernelShape = kernelShape;
            Strides = strides;
            Paddings = paddings;

            PaddedShape = Pad.Shape(inputShape, paddings);

            Kernels = new ArrayImmutable<Kernel>(nKernels, () => new Kernel(kernelShape));
        }

        private static Shape CalcOutShape(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings, int nKernels)
        {
            Validator.CheckParams(inputShape, kernelShape, strides, paddings);

            if (nKernels < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(nKernels));
            }

            ArrayImmutable<int> channelDims = inputShape.Dimensions.Select((D, i) => 1 + (D + 2 * paddings[i] - kernelShape.Dimensions[i]) / strides[i]);

            return new Shape(new ArrayImmutable<int>(nKernels).Concat(channelDims));
        }

        public override void Connect(ILayer inLayer)
        {
            if (inLayer.Shape != InputShape)
            {
                throw new ArgumentException("InLayer shape mismatch.");
            }

            ShapedArrayImmutable<Neuron> padded = Pad.ShapedArray(inLayer.Neurons, Paddings, () => new Neuron());

            var inConnections = new ShapedArrayImmutable<List<Synapse>>(PaddedShape, () => new List<Synapse>());

            Kernels.ForEach((kernel, i) =>
            {
                int offset = i * Shape.Volume / KernelsNum;

                IndexGen.ByStrides(PaddedShape, Strides, KernelShape).ForEach((idxKernel, j) =>
                {
                    var N = (CNeuron)Neurons[offset + j];

                    N.KernelBias = kernel.Bias;

                    N.InSynapses = IndexGen.ByStart(KernelShape, new ArrayImmutable<int>(idxKernel)).Select((idx, k) =>
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

            padded.ForEach((N, i) => N.OutSynapses = new ArrayImmutable<Synapse>(inConnections[i]));
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

        public override void Input(ShapedArrayImmutable<double> values)
        {
            throw new NotSupportedException("This layer doesn't support input.");
        }
    }
}