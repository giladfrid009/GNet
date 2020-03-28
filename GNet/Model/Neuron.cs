using System;

namespace GNet.Model
{
    [Serializable]
    public class Neuron
    {
        public ShapedArrayImmutable<Synapse> InSynapses { get; set; }
        public ShapedArrayImmutable<Synapse> OutSynapses { get; set; }
        public virtual double Bias { get; set; }

        public double Value;
        public double ActivatedValue;
        public double BatchBias;
        public double Gradient;
        public double Cache1;
        public double Cache2;

        public Neuron()
        {
            InSynapses = new ShapedArrayImmutable<Synapse>();
            OutSynapses = new ShapedArrayImmutable<Synapse>();
        }
    }
}