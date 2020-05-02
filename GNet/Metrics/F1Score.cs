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
            int i = 0;
            int nElems = 0;

            return 1.0 - targets.Sum(T =>
            {
                double O = outputs[i++] >= Threshold ? 1.0 : 0.0;

                if (T == 0.0 && O == 0.0)
                {
                    return 0.0;
                }

                if(T == 1.0 && O == 1.0)
                {
                    nElems += 2;
                    return 2.0;
                }

                nElems++;
                return 0.0;
            }) 
                / nElems;
        }
    }
}