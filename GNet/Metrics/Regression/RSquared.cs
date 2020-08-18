using NCollections;

namespace GNet.Metrics.Regression
{
    public class RSquared : IMetric
    {
        public double Evaluate(Array<double> targets, Array<double> outputs)
        {
            double avgT = targets.Sum(X => X) / targets.Length;

            return targets.Sum(outputs, (T, O) => (T - O) * (T - O)) / targets.Sum(T => (T - avgT) * (T - avgT));
        }
    }
}