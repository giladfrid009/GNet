using static System.Math;

namespace GNet.Optimizers.Decays
{
    public class Step : IDecay
    {
        public double Scale { get; }
        public int Interval { get; }

        public Step(double scale, int interval)
        {
            Scale = scale;
            Interval = interval;
        }

        public double Compute(double X, int T)
        {
            return X * Pow(Scale, T / Interval);
        }
    }
}