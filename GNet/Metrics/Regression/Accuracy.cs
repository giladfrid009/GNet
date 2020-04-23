namespace GNet.Metrics.Regression
{
    public class Accuracy : IMetric
    {
        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => T == O ? 0.0 : 1.0).Avarage();
        }
    }
}