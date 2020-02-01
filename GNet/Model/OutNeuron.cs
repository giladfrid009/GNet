using System;

namespace GNet.Model
{
    [Serializable]
    public class OutNeuron
    {
        public ShapedArrayImmutable<Synapse> OutSynapses { get; set; }

        public double ActivatedValue;
        public double Gradient;

        public OutNeuron()
        {

        }
    }
}
