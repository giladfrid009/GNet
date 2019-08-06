using System;

namespace GNet
{
    [Serializable]
    public class Synapse
    {
        // todo: maybe it should be IUnit InUnit and IUnit OutUnit. IUnit represents a neuron / a convolutional kernel.
        public Neuron InNeuron { get; }
        public Neuron OutNeuron { get; }
        public double Weight { get; set; }

        // training related
        public double Gradient;
        public double Cache1;
        public double Cache2;
        public double BatchWeight;

        public Synapse(Neuron inNeuron, Neuron outNeuron)
        {
            InNeuron = inNeuron;
            OutNeuron = outNeuron;
        }

        public void CloneVals(Synapse other)
        {
            Weight = other.Weight;
            Gradient = other.Gradient;
            Cache1 = other.Cache1;
            Cache2 = other.Cache2;
            BatchWeight = other.BatchWeight;
        }
    }
}
