namespace GNet
{
    public class Synapse
    {
        public Neuron InNeuron { get; }
        public Neuron OutNeuron { get; }
        public double Weight { get; set; }

        // training related
        public double Gradient { get; set; }
        public double SavedValue { get; set; }
        public double BatchWeight { get; set; }

        public Synapse(Neuron inNeuron, Neuron outNeuron)
        {
            InNeuron = inNeuron;
            OutNeuron = outNeuron;
        }
    }
}
