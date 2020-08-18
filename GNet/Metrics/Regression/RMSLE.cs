using NCollections;
using static System.Math;

namespace GNet.Metrics.Regression
{
    public class RMSLE : IMetric
    {
        private static readonly IMetric msle = new Losses.Regression.MSLE();

        public double Evaluate(NArray<double> targets, NArray<double> outputs)
        {
            return Sqrt(msle.Evaluate(targets, outputs));
        }
    }
}