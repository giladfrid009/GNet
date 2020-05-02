namespace GNet.Metrics
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

            int i = 0;

            return 1.0 - targets.Avarage(T =>
            {
                double O = outputs[i++] >= Threshold ? 1.0 : 0.0;

                return T == O ? 1.0 : 0.0;
            });
        }
    }
}