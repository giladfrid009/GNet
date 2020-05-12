using static System.Math;

namespace GNet.Metrics.Regression
{
    public class RMSE : IMetric
    {
        private static readonly IMetric mse;

        static RMSE()
        {
            mse = new Losses.Regression.MSE();
        }

        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return Sqrt(mse.Evaluate(targets, outputs));
        }
    }
}