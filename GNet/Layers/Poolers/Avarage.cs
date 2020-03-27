using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Avarage : IPooler
    {
        public ShapedArrayImmutable<double> GetWeights(ShapedArrayImmutable<double> inValues)
        {
            int nIn = inValues.Shape.Volume;
            return inValues.Select(X => 1.0 / nIn);
        }
    }
}