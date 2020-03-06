using System;

namespace GNet.Layers.Kernels
{
    [Serializable]
    public class MaxPool : IKernel
    {
        public bool IsTrainable { get; } = false;

        public ShapedArrayImmutable<double> InitWeights(ShapedArrayImmutable<double> inValues)
        {
            double max = inValues.Max();

            return inValues.Select(X => X == max ? 1.0 : 0.0);
        }

        public IKernel Clone()
        {
            return new MaxPool();
        }      
    }
}