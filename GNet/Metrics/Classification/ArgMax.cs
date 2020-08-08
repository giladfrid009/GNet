namespace GNet.Metrics.Classification
{
    public class ArgMax : IMetric
    {
        public double Evaluate(in ImmutableArray<double> targets, in ImmutableArray<double> outputs)
        {
            double maxVal = outputs.Max();

            return 1.0 - targets.Average(outputs, (T, O) => O == maxVal && T == 1.0 ? 1.0 : 0.0);
        }
    }
}