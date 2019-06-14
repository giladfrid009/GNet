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
        public double Cache1 { get; set; }
        public double Cache2 { get; set; }
        public double BatchBias { get; set; }
    }
}
