using NCollections;
namespace GNet.Metrics.Regression
{
    public class RSquared : IMetric
    {
        public double Evaluate(NArray<double> targets, NArray<double> outputs)
        {
            double avgT = targets.Average();

            double up = targets.Sum(outputs, (T, O) => (T - O) * (T - O), (T, O) => (T - O) * (T - O));
            
            double dn = targets.Sum(T => (T - avgT) * (T - avgT));

            return up / dn;
        }
    }
}