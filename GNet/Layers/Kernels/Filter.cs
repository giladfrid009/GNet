using System;
using GNet.Model;

namespace GNet.Layers.Kernels
{
    [Serializable]
    public class Filter : IKernel
    {
        public IInitializer Initializer { get; }
        public ShapedArrayImmutable<double> Weights { get; private set; }
        public Shape Shape { get; }

        public Filter(Shape shape, IInitializer initializer)
        {
            Shape = shape;
            Initializer = initializer.Clone();
            Weights = new ShapedArrayImmutable<double>(Shape, () => initializer.Initialize(Shape.Volume, 1));
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
            return new Filter(Shape, Initializer)
            {
                Weights = Weights
            };
        }
    }
}
