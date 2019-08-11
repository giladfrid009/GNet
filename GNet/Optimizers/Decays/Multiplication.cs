
namespace GNet.Optimizers.Decays
{
    using static System.Math;

    public class Multiplication : IDecay
    {
        public double Decay { get; }
        public double Multiplier { get; }
        public int Interval { get; }

        public Multiplication(double decay, int interval)
        {
            Decay = decay;
            Interval = interval;
            Multiplier = 1.0 - Decay;
        }

        public double Compute(double value, int iteration)
        {
            return value * Pow(Multiplier, iteration / Interval);
        }

        public IDecay Clone()
        {
            return new Multiplication(Decay, Interval);
        }
    }
}
