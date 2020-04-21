using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Min : IPooler
    {
        public double Pool(ImmutableArray<double> inVals, out ImmutableArray<double> inWeights)
        {
            double min = inVals.Min();

            inWeights = inVals.Select(X => X == min ? 1.0 : 0.0);

            return min;
        }
    }
}
