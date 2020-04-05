using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Min : IPooler
    {
        public double Pool(ArrayImmutable<double> vals, out ArrayImmutable<double> inWeights)
        {
            double min = vals.Min();

            inWeights = vals.Select(X => X == min ? 1.0 : 0.0);

            return min;
        }
    }
}
