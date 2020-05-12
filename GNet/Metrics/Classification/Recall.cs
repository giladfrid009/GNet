namespace GNet.Metrics.Classification
{
    public class Recall : IMetric
    {
        public double Threshold { get; }

        public Recall(double threshold = 0.5)
        {
            Threshold = threshold;
        }

        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            int i = 0;
            int nElems = 0;

            return 1.0 - targets.Sum(T =>
            {
                double O = outputs[i++] >= Threshold ? 1.0 : 0.0;

                if (T == 1.0)
                {
                    nElems++;

                    if(O >= Threshold)
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