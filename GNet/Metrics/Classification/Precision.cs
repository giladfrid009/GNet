using NCollections;

namespace GNet.Metrics.Classification
{
    public class Precision : IMetric
    {
        public double Threshold { get; }

        public Precision(double threshold = 0.5)
        {
            Threshold = threshold;
        }

        public double Evaluate(NArray<double> targets, NArray<double> outputs)
        {
            int nElems = 0;

            return 1.0 - targets.Sum(outputs, (T, O) =>
            {
                O = O >= Threshold ? 1.0 : 0.0;

                if (O >= Threshold)
                {
                    nElems++;

                    if (T == 1.0)
                    {
                        return 1.0;
                    }
                }

                return 0.0;
            })
                / nElems;
        }
    }
}