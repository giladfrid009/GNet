namespace GNet.Metrics.Classification
{
    public class Accuracy : IMetric
    {
        public double Threshold { get; }

        public Accuracy(double threshold = 0.5)
        {
            Threshold = threshold;
        }

        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs.Select(X => X >= Threshold ? 1.0 : 0.0), (T, O) => T == O ? 0.0 : 1.0).Avarage();
        }
    }
}