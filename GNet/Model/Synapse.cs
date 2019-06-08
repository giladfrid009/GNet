namespace GNet
{
    public class Synapse
    {
        public readonly Neuron InNeuron;
        public readonly Neuron OutNeuron;
        public double Weight;

        // training related
        public double Gradient = default;
        public double SavedValue = default;
        public double BatchWeight = default;

        public Synapse(Neuron inNeuron, Neuron outNeuron)
        {
            InNeuron = inNeuron;
            OutNeuron = outNeuron;
        }

        public void CalcGradient()
        {
            Gradient = OutNeuron.Gradient * InNeuron.ActivatedValue;
        }
    }
}
