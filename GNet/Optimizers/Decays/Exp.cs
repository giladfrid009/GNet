using static System.Math;

namespace GNet.Optimizers.Decays
{
    public class Exp : IDecay
    {
        public double Rate { get; }

        public Exp(double rate)
        {
            Rate = rate;
        }

        public double Compute(double X, int T)
        {
            return X * Exp(-Rate * T);
        }
    }
}