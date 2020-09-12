namespace GNet.Metrics.Regression
{
    public class RSquared : IMetric
    {
        public double Evaluate(Array<double> targets, Array<double> outputs)
        {
            double avgT = targets.Average(X => X);

            double up = targets.Sum(outputs, (T, O) => (T - O) * (T - O));

            double dn = targets.Sum(T => (T - avgT) * (T - avgT));

            return up / dn;
        }
    }
}