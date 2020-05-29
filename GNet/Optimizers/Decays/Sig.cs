using static System.Math;

namespace GNet.Optimizers.Decays
{
    public class Sig : IDecay
    {
        public double Rate { get; }
        public int Interval { get; }

        public Sig(double rate, int interval)
        {
            Rate = rate;
            Interval = interval;
        }

        public double Compute(double X, int T)
        {
            return X * 1.0 / (1.0 + Exp(Rate * (T - Interval)));
        }
    }
}
