using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Avarage : IPooler
    {
        public double Pool(ShapedArrayImmutable<double> vals, out ShapedArrayImmutable<double> inWeights)
        {
            int nIn = vals.Shape.Volume;

            inWeights = vals.Select(X => 1.0 / nIn);

            return vals.Avarage();
        }
    }
}