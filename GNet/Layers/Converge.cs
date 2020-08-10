using GNet.Model;
using System;

namespace GNet.Layers
{
    [Serializable]
    public class Converge : MergeLayer
    {
        public IOperation MergeOp { get; }

        public Converge(Shape shape, IOperation mergeOp) : base(shape)
        {
            MergeOp = mergeOp;
        }

        private void InitWeights()
        {
            Neurons.ForEach(N =>
            {
                Array<double> inWeights = MergeOp.CalcWeights(N.InSynapses);

                N.InSynapses.ForEach((S, i) => S.Weight = inWeights[i]);
            });
        }

        public override void Connect(Array<Layer> inLayers)
        {
            inLayers.ForEach((L, i) =>
            {
                if (L.Shape != Shape)
                {
                    throw new ShapeMismatchException($"{nameof(inLayers)}[{i}] shape mismatch.");
                }
            });

            Neurons.ForEach((outN, i) => outN.InSynapses = inLayers.Select(inL => new Synapse(inL.Neurons[i], outN)));

            inLayers.ForEach(inL => inL.Neurons.ForEach((inN, i) => inN.OutSynapses = new Array<Synapse>(new Synapse(inN, Neurons[i]))));
        }

        public override void Initialize()
        {
            if (MergeOp.RequiresUpdate == false)
            {
                InitWeights();
            }
        }

        public override void Forward(bool isTraining)
        {
            if (MergeOp.RequiresUpdate)
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