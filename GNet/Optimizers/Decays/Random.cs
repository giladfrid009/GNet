using GNet.Utils;
using static System.Math;

namespace GNet.Optimizers.Decays
{
    public class Random : IDecay
    {
        public double Power { get; }

        public Random(double power = 1.0)
        {
            Power = power;
        }

        public double Compute(double X, int T)
        {
            return X * Pow(GRandom.Uniform(), Power);
        }
    }
}