namespace GNet.Optimizers.Decays
{
    public class Sub : IDecay
    {
        public double Rate { get; }
        public int Interval { get; }

        public Sub(double rate, int interval)
        {
            Rate = rate;
            Interval = interval;
        }

        public double Compute(double X, int T)
        {
            return X - Rate * T / Interval;
        }
    }
}