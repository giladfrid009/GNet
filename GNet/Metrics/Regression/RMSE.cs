using NCollections;
using static System.Math;

namespace GNet.Metrics.Regression
{
    public class RMSE : IMetric
    {
        private static readonly IMetric mse = new Losses.Regression.MSE();

        public double Evaluate(NArray<double> targets, NArray<double> outputs)
        {
            return Sqrt(mse.Evaluate(targets, outputs));
        }
    }
}