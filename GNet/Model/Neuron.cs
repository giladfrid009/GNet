namespace GNet
{
    public class Neuron
    {
        public double Value { get; set; }
        public double ActivatedValue { get; set; }
        public double Bias { get; set; }
        public Synapse[] InSynapses { get; set; } = new Synapse[0];
        public Synapse[] OutSynapses { get; set; } = new Synapse[0];

        // training related
        public double Gradient { get; set; }
        public double SavedValue { get; set; }
        public double BatchBias { get; set; }
    }
}
