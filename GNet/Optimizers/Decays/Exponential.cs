using static System.Math;

namespace GNet.Optimizers.Decays
{
    public class Exponential : IDecay
    {
        public double Decay { get; }

        public Exponential(double decay)
        {
            Decay = decay;
        }

        public double Compute(double value, int iteration)
        {
            return value * Exp(-Decay * iteration);
        }

        public IDecay Clone()
        {
            return new Exponential(Decay);
        }
    }
}
