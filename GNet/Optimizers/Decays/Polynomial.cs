using static System.Math;

namespace GNet.Optimizers.Decays
{
    public class Polynomial : IDecay
    {
        public double Power { get; }
        public int MaxIter { get; }

        public Polynomial(int maxIter, double power)
        {
            MaxIter = maxIter;
            Power = power;
        }

        public double Compute(double X, int T)
        {
            return X * Pow(1.0 - T / MaxIter, Power);
        }
    }
}
