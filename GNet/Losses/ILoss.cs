namespace GNet
{
    public interface ILoss : IMetric
    {
        double Evaluate(double T, double O);

        double Derivative(double T, double O);

        double IMetric.Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Average(outputs, (T, O) => Evaluate(T, O));
        }
    }
}