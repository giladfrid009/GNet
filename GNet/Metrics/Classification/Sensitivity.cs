namespace GNet.Metrics.Classification
{
    public class Sensitivity : IMetric
    {
        public double Threshold { get; }

        public Sensitivity(double threshold = 0.5)
        {
            Threshold = threshold;
        }

        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            int div = 0;

            return targets.Combine(outputs, (T, O) =>
            {
                if(T == 1.0)
                {
                    div++;

                    if(O >= Threshold)
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