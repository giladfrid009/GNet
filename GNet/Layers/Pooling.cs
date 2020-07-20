using GNet.Model;
using GNet.Utils.Conv;
using System;
using System.Collections.Generic;

namespace GNet.Layers
{
    [Serializable]
    public class Pooling : ConstantLayer
    {
        public ImmutableArray<int> Strides { get; }
        public ImmutableArray<int> Paddings { get; }
        public Shape InputShape { get; }
        public Shape PaddedShape { get; }
        public Shape KernelShape { get; }
        public IPooler Pooler { get; }
        public double PadVal { get; }

        public Pooling(Shape inputShape, Shape outputShape, Shape kernelShape, ImmutableArray<int> strides, IPooler pooler, double padVal = 0.0) : base(outputShape)
        {
            InputShape = inputShape;
            KernelShape = kernelShape;
            Strides = strides;
            Pooler = pooler;
            PadVal = padVal;

            Paddings = Padder.CalcPadding(inputShape, outputShape, kernelShape, strides, true);

            PaddedShape = Padder.PadShape(inputShape, Paddings);
        }

        public override void Connect(Layer inLayer)
        {
            if (inLayer.Shape != InputShape)
            {
                throw new ShapeMismatchException(nameof(inLayer));
            }

            ImmutableShapedArray<Neuron> padded = Padder.PadShapedArray(inLayer.Neurons.ToShape(Shape), Paddings, () => new Neuron() { OutVal = PadVal });

            var inConnections = new ImmutableShapedArray<List<Synapse>>(PaddedShape, () => new List<Synapse>());

            IndexGen.ByStrides(PaddedShape, Strides, KernelShape).ForEach((idxKernel, i) =>
            {
                Neuron outN = Neurons[i];

                outN.InSynapses = IndexGen.ByStart(KernelShape, ImmutableArray<int>.FromRef(idxKernel)).Select((idx, j) =>
                {
                    var S = new Synapse(padded[idx], outN);
                    inConnections[idx].Add(S);
                    return S;
                });
            });

            padded.ForEach((N, i) => N.OutSynapses = ImmutableArray<Synapse>.FromRef(inConnections[i].ToArray()));
        }

        public override void Initialize()
        {
        }

        public override void Forward(bool isTraining)
        {
            Neurons.ForEach(N =>
            {
                ImmutableArray<double> inWeights = Pooler.CalcWeights(N.InSynapses);

                N.InSynapses.ForEach((S, i) => S.Weight = inWeights[i]);

                N.InVal = N.InSynapses.Sum(S => S.Weight * S.InNeuron.OutVal);

                N.OutVal = N.InVal;
            });
        }
    }
}