namespace GNet.Metrics
{
    public class ArgMaxAccuracy : IMetric
    {
        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            double max = outputs.Max();

            return targets.Combine(outputs.Select(X => X == max ? 1.0 : 0.0), (T, O) => T == O ? 0.0 : 1.0).Avarage();
        }
    }
}