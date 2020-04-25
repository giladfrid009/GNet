namespace GNet.Metrics
{
    public class Precision : IMetric
    {
        public double Threshold { get; }

        public Precision(double threshold = 0.5)
        {
            Threshold = threshold;
        }

        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            int div = 0;

            return targets.Combine(outputs, (T, O) =>
            {
                if (O >= Threshold)
                {
                    div++;

                    if(T == 1.0)
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