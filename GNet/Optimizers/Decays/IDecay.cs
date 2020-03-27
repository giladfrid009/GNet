namespace GNet.Optimizers
{
    public interface IDecay
    {
        double Decay { get; }

        double Compute(double value, int iteration);
    }
}