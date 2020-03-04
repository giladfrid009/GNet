namespace GNet.Optimizers.Decays
{
    public class Subtraction : IDecay
    {
        public double Decay { get; }
        public int Interval { get; }

        public Subtraction(double decay, int interval)
        {
            Decay = decay;
            Interval = interval;
        }

        public double Compute(double value, int iteration)
        {
            return value - Decay * (iteration / Interval);
        }

        public IDecay Clone()
        {
            return new Subtraction(Decay, Interval);
        }
    }
}