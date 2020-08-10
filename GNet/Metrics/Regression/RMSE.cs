using static System.Math;

namespace GNet.Metrics.Regression
{
    public class RMSE : IMetric
    {
        private static readonly IMetric mse = new Losses.Regression.MSE();

        public double Evaluate(Array<double> targets, Array<double> outputs)
        {
            return Sqrt(mse.Evaluate(targets, outputs));
        }
    }
}