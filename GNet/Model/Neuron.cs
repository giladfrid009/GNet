using System;
using NCollections;

namespace GNet.Model
{
    [Serializable]
    public class Neuron : TrainableObj
    {
        public Array<Synapse> InSynapses { get; set; }
        public Array<Synapse> OutSynapses { get; set; }
        public virtual double Bias { get; set; }
        public double InVal { get; set; }
        public double OutVal { get; set; }

        public Neuron()
        {
            InSynapses = new Array<Synapse>();
            OutSynapses = new Array<Synapse>();
        }
    }
}