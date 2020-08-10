namespace GNet.Metrics.Classification
{
    public class ArgMax : IMetric
    {
        public double Evaluate(Array<double> targets, Array<double> outputs)
        {
            double maxVal = outputs.Max(X => X);

            return 1.0 - targets.Average(outputs, (T, O) => O == maxVal && T == 1.0 ? 1.0 : 0.0);
        }
    }
}