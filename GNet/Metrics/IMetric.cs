namespace GNet
{
    public interface IMetric
    {
        double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs);
    }
}