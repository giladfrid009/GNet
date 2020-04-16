using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Avarage : IPooler
    {
        public double Pool(ImmutableArray<double> vals, out ImmutableArray<double> inWeights)
        {
            int nIn = vals.Length;

            inWeights = vals.Select(X => 1.0 / nIn);

            return vals.Avarage();
        }
    }
}