using static System.Math;

namespace GNet.Optimizers.Decays
{
    public class Mul : IDecay
    {
        public double Scale { get; }

        public Mul(double rate)
        {
            Scale = rate;
        }

        public double Compute(double X, int T)
        {
            return X * Pow(Scale, T);
        }
    }
}