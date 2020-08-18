using NCollections;
using System.Numerics;

namespace GNet.Metrics.Regression
{
    public class RSquared : IMetric
    {
        public double Evaluate(NArray<double> targets, NArray<double> outputs)
        {
            double avgT = targets.Average();
            var vAvgT = new Vector<double>(avgT);

            double up = targets.Sum(outputs, (T, O) => (T - O) * (T - O), (T, O) => (T - O) * (T - O));
            
            double dn = targets.Sum(T => (T - vAvgT) * (T - vAvgT), T => (T - avgT) * (T - avgT));

            return up / dn;
        }
    }
}