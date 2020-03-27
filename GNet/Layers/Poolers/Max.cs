using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Max : IPooler
    {
        public ShapedArrayImmutable<double> GetWeights(ShapedArrayImmutable<double> inValues)
        {
            double max = inValues.Max();

            return inValues.Select(X => X == max ? 1.0 : 0.0);
        }    
    }
}