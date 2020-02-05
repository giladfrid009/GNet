using System;

namespace GNet.Model
{
    [Serializable]
    public class Synapse
    {
        // todo: come up with better naming.
        public OutNeuron InNeuron { get; }
        public InNeuron OutNeuron { get; }
        public double Weight { get; set; }

        public double Gradient;
        public double Cache1;
        public double Cache2;
        public double BatchWeight;

        public Synapse(OutNeuron inNeuron, InNeuron outNeuron)
        {
            InNeuron = inNeuron;
            OutNeuron = outNeuron;
        }       
    }
}
