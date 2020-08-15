using GNet.Model;
using GNet.Utils.Conv;
using System;
using System.Collections.Generic;

namespace GNet.Layers
{
    [Serializable]
    public class Pooling : ConstantLayer
    {
        public VArray<int> Strides { get; }
        public VArray<int> Paddings { get; }
        public Shape InputShape { get; }
        public Shape PaddedShape { get; }
        public Shape KernelShape { get; }
        public IOperation PoolOp { get; }
        public double PadVal { get; }

        public Pooling(Shape inputShape, Shape outputShape, Shape kernelShape, VArray<int> strides, IOperation poolOp, double padVal = 0.0) : base(outputShape)
        {
            InputShape = inputShape;
            KernelShape = kernelShape;
            Strides = strides;
            PoolOp = poolOp;
            PadVal = padVal;

            Paddings = Padder.CalcPadding(inputShape, outputShape, kernelShape, strides, true);

            PaddedShape = inputShape.Pad(Paddings);
        }

        private void InitWeights()
        {
            Neurons.ForEach(N =>
            {
                Array<double> inWeights = PoolOp.CalcWeights(N.InSynapses);

                N.InSynapses.ForEach((S, i) => S.Weight = inWeights[i]);
            });
        }

        public override void Connect(Layer inLayer)
        {
            if (inLayer.Shape != InputShape)
            {
                throw new ShapeMismatchException(nameof(inLayer));
            }

            ShapedArray<Neuron> padded = Padder.PadShapedArray(inLayer.Neurons.ToShape(Shape), Paddings, () => new Neuron() { OutVal = PadVal });

            var inConnections = new ShapedArray<List<Synapse>>(PaddedShape, () => new List<Synapse>());

            IndexGen.ByStrides(PaddedShape, Strides, KernelShape).ForEach((idxKernel, i) =>
            {
                Neuron outN = Neurons[i];

                outN.InSynapses = IndexGen.ByStart(KernelShape, VArray<int>.FromRef(idxKernel)).Select((idx, j) =>
                {
                    var S = new Synapse(padded[idx], outN);
                    inConnections[idx].Add(S);
                    return S;
                });
            });

            padded.ForEach((N, i) => N.OutSynapses = Array<Synapse>.FromRef(inConnections[i].ToArray()));
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