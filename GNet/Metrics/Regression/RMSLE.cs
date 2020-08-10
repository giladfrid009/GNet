using static System.Math;

namespace GNet.Metrics.Regression
{
    public class RMSLE : IMetric
    {
        private static readonly IMetric msle = new Losses.Regression.MSLE();

        public double Evaluate(Array<double> targets, Array<double> outputs)
        {
            return Sqrt(msle.Evaluate(targets, outputs));
        }
    }
}