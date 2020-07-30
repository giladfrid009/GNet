using static System.Math;

namespace GNet.Optimizers.Decays
{
    public class Polynomial : IDecay
    {
        public double Power { get; }
        public double Dest { get; }
        public int MaxIter { get; }

        public Polynomial(int maxIter, double dest, double power = 1.0)
        {
            MaxIter = maxIter;
            Dest = dest;
            Power = power;
        }

        public double Compute(double X, int T)
        {
            if (T > MaxIter)
            {
                return Dest;
            }

            return (X - Dest) * Pow(1.0 - T / MaxIter, Power) + Dest;
        }
    }
}
