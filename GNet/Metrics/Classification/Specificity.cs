namespace GNet.Metrics.Classification
{
    public class Specificity : IMetric
    {
        public double Threshold { get; }

        public Specificity(double threshold = 0.5)
        {
            Threshold = threshold;
        }

        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            int div = 0;

            return targets.Combine(outputs, (T, O) =>
            {
                if (T == 0.0)
                {
                    div++;

                    if (O < Threshold)
                    {
                        return 1.0;
                    }
                }

                return 0.0;
            })
            .Sum() / div;
        }
    }
}