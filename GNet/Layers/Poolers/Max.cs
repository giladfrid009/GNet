using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Max : IPooler
    {
        public double Pool(ArrayImmutable<double> vals, out ArrayImmutable<double> inWeights)
        {
            double max = vals.Max();

            inWeights = vals.Select(X => X == max ? 1.0 : 0.0);

            return max;
        }
    }
}