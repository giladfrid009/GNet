using System;
using GNet.Model;

namespace GNet.Layers.Kernels
{
    [Serializable]
    public class Filter : IKernel
    {
        public IInitializer WeightInit { get; }
        public ShapedArrayImmutable<double> Weights { get; private set; }
        public Shape Shape { get; private set; }
        public bool IsTrainable { get; set; } = true;

        public Filter(IInitializer weightInit)
        {
            WeightInit = weightInit.Clone();
        }

        public void Initialize(Shape shape)
        {
            Shape = shape;
            Weights = new ShapedArrayImmutable<double>(shape, () => WeightInit.Initialize(Shape.Volume, 1));
        }

        public void Update(ShapedArrayImmutable<Synapse> inSynapses)
        {
            if (inSynapses.Shape != Shape)
            {
                throw new ArgumentException("InSynapses shape mismatch.");
            }

            Weights = Weights.Select((W, i) => W + inSynapses[i].BatchWeight);
        }

        public IKernel Clone()
        {
            return new Filter(WeightInit)
            {
                Shape = Shape,
                Weights = Weights,
                IsTrainable = IsTrainable
            };
        }
    }
}