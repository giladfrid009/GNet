namespace GNet.Optimizers
{
    public interface IDecay : ICloneable<IDecay>
    {
        double Decay { get; }

        double Compute(double value, int iteration);
    }
}
