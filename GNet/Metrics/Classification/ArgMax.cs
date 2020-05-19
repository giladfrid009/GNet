namespace GNet.Metrics.Classification
{
    public class ArgMax : IMetric
    {
        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            int i = 0;

            double max = outputs.Max();

            return 1.0 - targets.Avarage(T => T == 1.0 && outputs[i++] == max ? 1.0 : 0.0);
        }
    }
}