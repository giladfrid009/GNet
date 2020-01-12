using System;

namespace GNet
{
    [Serializable]
    public class InNeuron
    {
        public ShapedArrayImmutable<Synapse> InSynapses { get; set; }
        public double Bias { get; set; }

        public double Value;
        public double Gradient;
        public double Cache1;
        public double Cache2;
        public double BatchBias;

        public InNeuron()
        {

        }
    }
}

