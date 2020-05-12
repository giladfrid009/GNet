using static System.Math;

namespace GNet.Metrics.Regression
{
    public class RMSLE : IMetric
    {
        private static readonly IMetric msle;

        static RMSLE()
        {
            msle = new Losses.Regression.MSLE();
        }

        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return Sqrt(msle.Evaluate(targets, outputs));
        }
    }
}