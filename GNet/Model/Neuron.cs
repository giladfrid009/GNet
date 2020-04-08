using System;

namespace GNet.Model
{
    [Serializable]
    public class Neuron : IOptimizable
    {
        public ArrayImmutable<Synapse> InSynapses { get; set; }
        public ArrayImmutable<Synapse> OutSynapses { get; set; }
        public virtual double Bias { get; set; }
        public double Value { get; set; }
        public double ActivatedValue { get; set; }
        public double Gradient { get; set; }
        public double Cache1 { get; set; }
        public double Cache2 { get; set; }
        public double BatchDelta { get; set; }   

        public Neuron()
        {
            InSynapses = new ShapedArrayImmutable<Synapse>();
            OutSynapses = new ShapedArrayImmutable<Synapse>();
        }
    }
}