namespace GNet.Optimizers.Decays
{
    public class IterBased : IDecay
    {
        public double Decay { get; }

        public IterBased(double decay)
        {
            Decay = decay;
        }

        public double Compute(double value, int iteration)
        {
            return value / (1.0 + Decay * iteration);
        }
    }
}