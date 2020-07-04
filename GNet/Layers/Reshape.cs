using GNet.Model;
using System;

namespace GNet.Layers
{
    [Serializable]
    public class Reshape : LayerConst
    {
        public Reshape(Shape shape) : base(shape)
        {
        }

        public override void Connect(Layer inLayer)
        {
            if (inLayer.Shape.Volume != Shape.Volume)
            {
                throw new ShapeMismatchException($"{nameof(inLayer)} shape volume mismatch.");
            }

            Neurons.ForEach((N, i) =>
            {
                var S = new Synapse(inLayer.Neurons[i], N);
                N.InSynapses = new ImmutableArray<Synapse>(S);
                inLayer.Neurons[i].OutSynapses = new ImmutableArray<Synapse>(S);
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
