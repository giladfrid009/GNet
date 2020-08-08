namespace GNet
{
    public interface IMetric
    {
        double Evaluate(in ImmutableArray<double> targets, in ImmutableArray<double> outputs);
    }
}