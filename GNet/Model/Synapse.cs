namespace GNet
{
    public class Synapse
    {
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

        public static Synapse Like(Neuron inNeuron, Neuron outNeuron, Synapse other)
        {
            return new Synapse(inNeuron, outNeuron)
            {
                Weight = other.Weight,
                Gradient = other.Gradient,
                Cache1 = other.Cache1,
                Cache2 = other.Cache2,
                BatchWeight = other.BatchWeight
            };
        }
    }
}
