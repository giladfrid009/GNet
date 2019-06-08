using GNet.Extensions;

namespace GNet
{
    public class Neuron
    {
        public double Value;
        public double ActivatedValue;
        public double Bias;
        public Synapse[] InSynapses = new Synapse[0];
        public Synapse[] OutSynapses = new Synapse[0];

        // training related
        public double Gradient = default;
        public double SavedValue = default;
        public double BatchBias = default;

        public Neuron()
        {

        }
    }
}
