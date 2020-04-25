namespace GNet.Metrics
{
    public class F1Score : IMetric
    {
        public double Threshold { get; }

        public F1Score(double threshold = 0.5)
        {
            Threshold = threshold;
        }

        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            int div = 0;

            return targets.Combine(outputs, (T, O) =>
            {
                if (T == 0.0 && O == 0.0)
                {
                    return 0.0;
                }

                if(T == 1.0 && O == 1.0)
                {
                    div += 2;
                    return 2.0;
                }

                div++;
                return 0.0;
            })
            .Sum() / div;
        }
    }
}