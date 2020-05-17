using System;

namespace GNet.Model
{
    [Serializable]
    public class Synapse : TrainableObj
    {
        public Neuron InNeuron { get; }
        public Neuron OutNeuron { get; }
        public virtual double Weight { get; set; }

        public Synapse(Neuron inNeuron, Neuron outNeuron)
        {
            InNeuron = inNeuron;
            OutNeuron = outNeuron;
        }
    }
}