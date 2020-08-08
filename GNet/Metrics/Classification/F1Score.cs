namespace GNet.Metrics.Classification
{
    public class F1Score : IMetric
    {
        public double Threshold { get; }

        public F1Score(double threshold = 0.5)
        {
            Threshold = threshold;
        }

        public double Evaluate(in ImmutableArray<double> targets, in ImmutableArray<double> outputs)
        {
            int nElems = 0;

            return 1.0 - targets.Sum(outputs, (T, O) =>
            {
                O = O >= Threshold ? 1.0 : 0.0;

                if (T == 0.0 && O == 0.0)
                {
                    return 0.0;
                }

                if (T == 1.0 && O == 1.0)
                {
                    nElems += 2;
                    return 2.0;
                }

                nElems++;
                return 0.0;
            })
                / nElems;
        }
    }
}