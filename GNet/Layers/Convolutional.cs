using GNet.Model;
using GNet.Model.Conv;
using GNet.Utils.Conv;
using System;
using System.Collections.Generic;

namespace GNet.Layers
{
    [Serializable]
    public class Convolutional : TrainableLayer
    {
        public override Array<Neuron> Neurons { get; }
        public Array<Kernel> Kernels { get; }
        public Array<int> Strides { get; }
        public Array<int> Paddings { get; }
        public Shape InputShape { get; }
        public Shape PaddedShape { get; }
        public Shape KernelShape { get; }
        public int KernelsNum { get; }
        public double PadVal { get; }

        public Convolutional(Shape inputShape, Shape outputShape, Shape kernelShape, Array<int> strides, IActivation activation,
            IInitializer? weightInit = null, IInitializer? biasInit = null, double padVal = 0.0)
            : base(outputShape, activation, weightInit, biasInit)
        {
            ValidateChannels(inputShape, outputShape, kernelShape, strides);

            InputShape = inputShape;
            KernelShape = kernelShape;
            Strides = strides;
            PadVal = padVal;

            Paddings = Padder.CalcPadding(inputShape, outputShape, kernelShape, strides, false);

            PaddedShape = Padder.PadShape(inputShape, Paddings);

            KernelsNum = outputShape.Dims[0] / inputShape.Dims[0];

            Kernels = new Array<Kernel>(KernelsNum, () => new Kernel(kernelShape));

            Neurons = new Array<Neuron>(outputShape.Volume, () => new CNeuron());
        }

        private static void ValidateChannels(Shape inputShape, Shape outputShape, Shape kernelShape, Array<int> strides)
        {
            if (strides[0] != 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(strides)} channels dim is not 1.");
            }

            if (kernelShape.Dims[0] != 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(kernelShape)} channels dim is not 1.");
            }

            if (outputShape.Dims[0] < inputShape.Dims[0] || outputShape.Dims[0] % inputShape.Dims[0] != 0)
            {
                throw new ShapeMismatchException($"{nameof(outputShape)} channel dim params are invalid.");
            }
        }

        public override void Connect(Layer inLayer)
        {
            if (inLayer.Shape != InputShape)
            {
                throw new ShapeMismatchException(nameof(inLayer));
            }

            ShapedArray<Neuron> padded = Padder.PadShapedArray(inLayer.Neurons.ToShape(Shape), Paddings, () => new Neuron() { OutVal = PadVal });

            var inConnections = new ShapedArray<List<Synapse>>(PaddedShape, () => new List<Synapse>());

            Kernels.ForEach((kernel, i) =>
            {
                int offset = i * Shape.Volume / KernelsNum;

                IndexGen.ByStrides(PaddedShape, Strides, KernelShape).ForEach((idxKernel, j) =>
                {
                    var outN = (CNeuron)Neurons[offset + j];

                    outN.KernelBias = kernel.Bias;

                    outN.InSynapses = IndexGen.ByStart(KernelShape, Array<int>.FromRef(idxKernel)).Select((idx, k) =>
                    {
                        var S = new CSynapse(padded[idx], outN)
                        {
                            KernelWeight = kernel.Weights[k]
                        };

                        inConnections[idx].Add(S);
                        return (Synapse)S;
                    });
                });
            });

            padded.ForEach((N, i) => N.OutSynapses = Array<Synapse>.FromRef(inConnections[i].ToArray()));
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
    }
}