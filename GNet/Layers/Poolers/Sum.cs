using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Sum : IPooler
    {
        public double Pool(ImmutableArray<double> inVals, out ImmutableArray<double> inWeights)
        {
            inWeights = inVals.Select(X => 1.0);

            return inVals.Sum();
        }
    }
}