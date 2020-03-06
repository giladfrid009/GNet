using System;
using GNet.Model;

namespace GNet.Layers.Kernels
{
    [Serializable]
    public class AvaragePool : IKernel
    {
        public ShapedArrayImmutable<double> Weights { get; private set; }
        public Shape Shape { get; private set; }
        public bool IsTrainable { get; } = false;

        public void Initialize(Shape shape)
        {
            Shape = shape;
            Weights = new ShapedArrayImmutable<double>(shape, () => 1.0 / shape.Volume);
        }

        public void Update(ShapedArrayImmutable<Synapse> inSynapses)
        {
        }

        public IKernel Clone()
        {
            return new AvaragePool()
            {
                Shape = Shape,
                Weights = Weights
            };
        }
    }
}