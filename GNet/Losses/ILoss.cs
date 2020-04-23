namespace GNet
{
    public interface ILoss : IMetric
    {
        ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs);
    }
}