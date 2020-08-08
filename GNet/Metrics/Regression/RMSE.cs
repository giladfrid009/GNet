using static System.Math;

namespace GNet.Metrics.Regression
{
    public class RMSE : IMetric
    {
        private static readonly IMetric mse = new Losses.Regression.MSE();

        public double Evaluate(in ImmutableArray<double> targets, in ImmutableArray<double> outputs)
        {
            return Sqrt(mse.Evaluate(targets, outputs));
        }
    }
}