using GNet.Model;
using System;

namespace GNet.Layers
{
    [Serializable]
    public class Reshape : ConstantLayer
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

            Neurons.ForEach((outN, i) =>
            {
                Neuron inN = inLayer.Neurons[i];
                var S = new Synapse(inN, outN);
                var arr = new Array<Synapse>(S);
                outN.InSynapses = arr;
                inN.OutSynapses = arr;
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