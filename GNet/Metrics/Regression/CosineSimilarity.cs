using static System.Math;

namespace GNet.Metrics.Regression
{
    class CosineSimilarity : IMetric
    {
        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            int i = 0;

            double dotProd = targets.Sum(T => T * outputs[i++]);

            return dotProd / (Sqrt(targets.Sum(T => T * T)) + Sqrt(outputs.Sum(O => O * O)));
        }
    }
}
