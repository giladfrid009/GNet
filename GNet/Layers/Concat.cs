using GNet.Model;
using System;

namespace GNet.Layers
{
    [Serializable]
    public class Concat : MergeLayer
    {
        public Concat(Shape shape) : base(shape)
        {
        }

        public override void Connect(ImmutableArray<Layer> inLayers)
        {
            if(inLayers.Sum(L => L.Shape.Volume) != Shape.Volume)
            {
                throw new ShapeMismatchException($"{nameof(inLayers)} shapes volume mismatch.");
            }

            int i = 0;
            inLayers.ForEach(L =>
            {
                L.Neurons.ForEach(inN =>
                {
                    Neuron outN = Neurons[i];
                    var S = new Synapse(inN, outN);
                    var arr = new ImmutableArray<Synapse>(S);
                    inN.OutSynapses = arr;
                    outN.InSynapses = arr;
                    i++;
                });
            });
        }

        public override void Initialize()
        {
            Neurons.ForEach(N => N.InSynapses[0].Weight = 1.0);
        }

        public override void Forward(bool isTraining)
        {
            Neurons.ForEach((N, i) =>
            {
                N.InVal = N.InSynapses[0].InNeuron.OutVal;
                N.OutVal = N.InVal;
            });
        }
    }
}
