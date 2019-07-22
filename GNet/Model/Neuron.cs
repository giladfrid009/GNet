namespace GNet
{
    public class Neuron
    {
        public Synapse[] InSynapses { get; set; } = new Synapse[0];
        public Synapse[] OutSynapses { get; set; } = new Synapse[0];
        public double Bias { get; set; }

        public double Value;
        public double ActivatedValue;

        // training related
        public double Gradient;
        public double Cache1;
        public double Cache2;
        public double BatchBias;

        public void CloneVals(Neuron other)
        {
            Value = other.Value;
            ActivatedValue = other.ActivatedValue;
            Bias = other.Bias;
            Gradient = other.Gradient;
            Cache1 = other.Cache1;
            Cache2 = other.Cache2;
            BatchBias = other.BatchBias;
        }
    }
}
