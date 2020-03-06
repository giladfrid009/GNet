using System;

namespace GNet.Layers.Kernels
{
    [Serializable]
    public class AvaragePool : IKernel
    {
       public bool IsTrainable { get; } = false;

        public ShapedArrayImmutable<double> InitWeights(ShapedArrayImmutable<double> inValues)
        {
            int nIn = inValues.Shape.Volume;
            return inValues.Select(X => 1.0 / nIn);
        }

        public IKernel Clone()
        {
            return new AvaragePool();
        }
    }
}