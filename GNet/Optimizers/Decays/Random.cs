using GNet.Utils;
using static System.Math;

namespace GNet.Optimizers.Decays
{
    public class Random : IDecay
    {
        public double Power { get; }

        public Random(double power)
        {
            Power = power;
        }

        public double Compute(double X, int T)
        {
            return X * Pow(GRandom.Uniform(), Power);
        }
    }
}
