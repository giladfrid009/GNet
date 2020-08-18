using NCollections;
using static System.Math;

namespace GNet.Metrics.Regression
{
    public class CosineSimilarity : IMetric
    {
        public double Evaluate(NArray<double> targets, NArray<double> outputs)
        {
            double dotProd = targets.Dot(outputs);

            return dotProd / (Sqrt(targets.Sum(T => T * T, T => T * T)) + Sqrt(outputs.Sum(O => O * O, O => O * O)));
        }
    }
}