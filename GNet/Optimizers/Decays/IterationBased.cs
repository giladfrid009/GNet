namespace GNet.Optimizers.Decays
{
    public class IterationBased : IDecay
    {
        public double Decay { get; }

        public IterationBased(double decay)
        {
            Decay = decay;
        }

        public double Compute(double value, int iteration)
        {
            return value / (1.0 + Decay * iteration);
        }

        public IDecay Clone()
        {
            return new IterationBased(Decay);
        }
    }
}