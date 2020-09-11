using static System.Math;

namespace GNet.Metrics.Regression
{
    public class CosineSimilarity : IMetric
    {
        public double Evaluate(Array<double> targets, Array<double> outputs)
        {
            return targets.Sum(outputs, (T, O) => T * O) / (Sqrt(targets.Sum(T => T * T)) + Sqrt(outputs.Sum(O => O * O)));
        }
    }
}