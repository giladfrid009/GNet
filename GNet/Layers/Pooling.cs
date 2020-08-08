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
        public IOperation PoolOp { get; }
        public double PadVal { get; }

        public Pooling(in Shape inputShape, in Shape outputShape, in Shape kernelShape, in ImmutableArray<int> strides, IOperation poolOp, double padVal = 0.0) : base(outputShape)
        {
            InputShape = inputShape;
            KernelShape = kernelShape;
            Strides = strides;
            PoolOp = poolOp;
            PadVal = padVal;

            Paddings = Padder.CalcPadding(inputShape, outputShape, kernelShape, strides, true);

            PaddedShape = Padder.PadShape(inputShape, Paddings);
        }

        private void InitWeights()
        {
            Neurons.ForEach(N =>
            {
                ImmutableArray<double> inWeights = PoolOp.CalcWeights(N.InSynapses);

                N.InSynapses.ForEach((S, i) => S.Weight = inWeights[i]);
            });
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
            if (PoolOp.RequiresUpdate == false)
            {
                InitWeights();
            }
        }

        public override void Forward(bool isTraining)
        {
            if (PoolOp.RequiresUpdate)
            {
                InitWeights();
            }

            Neurons.ForEach(N =>
            {
                N.InVal = N.InSynapses.Sum(S => S.Weight * S.InNeuron.OutVal);
                N.OutVal = N.InVal;
            });
        }
    }
}