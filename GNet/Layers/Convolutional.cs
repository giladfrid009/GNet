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

        public Convolutional(Shape inputShape, Shape outputShape, Shape kernelShape, ImmutableArray<int> strides, IActivation activation, IInitializer? weightInit = null, IInitializer? biasInit = null) : base(activation, weightInit, biasInit)
        {
            if(strides[0] != 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(strides)} [0] is out of range. It must be 1.");
            }

            if(kernelShape.Dims[0] != 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(kernelShape)} {nameof(kernelShape.Dims)} [0] is out of range. It must be 1.");
            }

            int inChannels = inputShape.Dims[0];
            int outChannels = outputShape.Dims[0];

            if (outChannels < inChannels || outChannels % inChannels != 0)
            {
                throw new ShapeMismatchException(nameof(outputShape));
            }

            InputShape = inputShape;
            Shape = outputShape;
            KernelShape = kernelShape;
            Strides = strides;

            Paddings = Padder.CalcPadding(inputShape, outputShape, kernelShape, strides, false);

            PaddedShape = Padder.PadShape(inputShape, Paddings);

            KernelsNum = outChannels / inChannels;

            Kernels = new ImmutableArray<Kernel>(KernelsNum, () => new Kernel(kernelShape));

            Neurons = new ImmutableShapedArray<Neuron>(outputShape, () => new CNeuron());
        }

        public override void Connect(ILayer inLayer)
        {
            if (inLayer.Shape != InputShape)
            {
                throw new ShapeMismatchException(nameof(inLayer));
            }

            ImmutableShapedArray<Neuron> padded = Padder.PadShapedArray(inLayer.Neurons, Paddings, () => new Neuron());

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

            padded.ForEach((N, i) => N.OutSynapses = new ImmutableArray<Synapse>(inConnections[i].ToArray()));
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