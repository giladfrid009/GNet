namespace GNet.Metrics.Classification
{
    public class Recall : IMetric
    {
        public double Threshold { get; }

        public Recall(double threshold = 0.5)
        {
            Threshold = threshold;
        }

        public double Evaluate(in ImmutableArray<double> targets, in ImmutableArray<double> outputs)
        {
            int nElems = 0;

            return 1.0 - targets.Sum(outputs, (T, O) =>
            {
                if (T == 1.0)
                {
                    nElems++;

                    if (O >= Threshold)
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