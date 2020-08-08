using static System.Math;

namespace GNet.Metrics.Regression
{
    public class CosineSimilarity : IMetric
    {
        public double Evaluate(in ImmutableArray<double> targets, in ImmutableArray<double> outputs)
        {
            double dotProd = targets.Sum(outputs, (T, O) => T * O);

            return dotProd / (Sqrt(targets.Sum(T => T * T)) + Sqrt(outputs.Sum(O => O * O)));
        }
    }
}