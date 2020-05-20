﻿namespace GNet.Metrics.Classification
{
    public class ArgMax : IMetric
    {
        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            double max = outputs.Max();

            return 1.0 - targets.Avarage(outputs, (T, O) => O == max && T == 1.0 ? 1.0 : 0.0);
        }
    }
}