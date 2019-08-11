
namespace GNet.Optimizers.Decays
{
    public class None : IDecay
    {
        public double Decay { get; } = 0;

        public double Compute(double value, int iteration)
        {
            return value;
        }

        public IDecay Clone()
        {
            return new None();
        }
    }
}
