using System;

namespace GNet.Model
{
    [Serializable]
    public class OutNeuron : ICloneable<OutNeuron>
    {
        public ShapedArrayImmutable<Synapse> OutSynapses { get; set; }

        public double ActivatedValue;
        public double Gradient;

        public OutNeuron()
        {

        }

        public OutNeuron Clone()
        {
            return new OutNeuron()
            {
                ActivatedValue = ActivatedValue,
                Gradient = Gradient
            };
        }
    }
}
