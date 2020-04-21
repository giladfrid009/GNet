using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Max : IPooler
    {
        public double Pool(ImmutableArray<double> inVals, out ImmutableArray<double> inWeights)
        {
            double max = inVals.Max();

            inWeights = inVals.Select(X => X == max ? 1.0 : 0.0);

            return max;
        }
    }
}