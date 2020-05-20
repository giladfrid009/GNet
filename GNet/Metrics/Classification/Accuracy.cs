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
            return 1.0 - targets.Avarage(outputs, (T, O) =>
            {
                O = O >= Threshold ? 1.0 : 0.0;
                return T == O ? 1.0 : 0.0;
            });
        }
    }
}