using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Avarage : IPooler
    {
        public double Pool(ImmutableArray<double> inVals, out ImmutableArray<double> inWeights)
        {
            int nIn = inVals.Length;

            inWeights = inVals.Select(X => 1.0 / nIn);

            return inVals.Avarage();
        }
    }
}