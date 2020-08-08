using static System.Math;

namespace GNet.Metrics.Regression
{
    public class RMSLE : IMetric
    {
        private static readonly IMetric msle = new Losses.Regression.MSLE();

        public double Evaluate(in ImmutableArray<double> targets, in ImmutableArray<double> outputs)
        {
            return Sqrt(msle.Evaluate(targets, outputs));
        }
    }
}