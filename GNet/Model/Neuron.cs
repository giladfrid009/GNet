using System;

namespace GNet.Model
{
    [Serializable]
    public class Neuron : TrainableObj
    {
        public ImmutableArray<Synapse> InSynapses { get; set; }
        public ImmutableArray<Synapse> OutSynapses { get; set; }
        public virtual double Bias { get; set; }
        public double InVal { get; set; }
        public double OutVal { get; set; }

        public Neuron()
        {
            InSynapses = new ImmutableArray<Synapse>();
            OutSynapses = new ImmutableArray<Synapse>();
        }
    }
}