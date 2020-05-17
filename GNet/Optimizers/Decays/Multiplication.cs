using static System.Math;

namespace GNet.Optimizers.Decays
{
    public class Multiplication : IDecay
    {
        public double Decay { get; }
        public int Interval { get; }

        public Multiplication(double decay, int interval)
        {
            Decay = decay;
            Interval = interval;
        }

        public double Compute(double value, int iteration)
        {
            return value * Pow(Decay, (double)iteration / Interval);
        }
    }
}