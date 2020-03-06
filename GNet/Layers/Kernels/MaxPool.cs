using System;
using GNet.Model;

namespace GNet.Layers.Kernels
{
    [Serializable]
    public class MaxPool : IKernel
    {
        public ShapedArrayImmutable<double> Weights { get; private set; }
        public Shape Shape { get; private set; }
        public bool IsTrainable { get; } = false;

        public void Initialize(Shape shape)
        {
            Shape = shape;
            Weights = new ShapedArrayImmutable<double>(shape, () => 0.0);
        }

        public void Update(ShapedArrayImmutable<Synapse> inSynapses)
        {
            if (inSynapses.Shape != Shape)
            {
                throw new ArgumentException("InSynapses shape mismatch.");
            }

            double max = inSynapses.Select(S => S.InNeuron.ActivatedValue).Max();

            Weights = inSynapses.Select(S => S.InNeuron.ActivatedValue == max ? 1.0 : 0.0);
        }

        public IKernel Clone()
        {
            return new MaxPool()
            {
                Shape = Shape,
                Weights = Weights
            };
        }
    }
}