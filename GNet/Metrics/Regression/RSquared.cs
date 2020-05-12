namespace GNet.Metrics.Regression
{
    public class RSquared : IMetric
    {
        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            int i = 0;

            double mOuts = outputs.Avarage();

            return targets.Sum(T =>
            {
                double O = outputs[i++];

                return (T - O) * (T - O);
            })
            / targets.Sum(T => (T - mOuts) * (T - mOuts));
        }
    }
}
