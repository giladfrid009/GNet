using NCollections;

namespace GNet.Metrics.Classification
{
    public class ArgMax : IMetric
    {
        public double Evaluate(NArray<double> targets, NArray<double> outputs)
        {
            double maxVal = outputs.Max();

            return 1.0 - targets.Average(outputs, (T, O) => O == maxVal && T == 1.0 ? 1.0 : 0.0);
        }
    }
}