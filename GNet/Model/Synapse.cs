using System;

namespace GNet.Model
{
    [Serializable]
    public class Synapse : IOptimizable
    {
        public Neuron InNeuron { get; }
        public Neuron OutNeuron { get; }
        public virtual double Weight { get; set; }
        public double Gradient { get; set; }
        public double Cache1 { get; set; }
        public double Cache2 { get; set; }
        public double BatchDelta { get; set; }

        public Synapse(Neuron inNeuron, Neuron outNeuron)
        {
            InNeuron = inNeuron;
            OutNeuron = outNeuron;
        }
    }
}