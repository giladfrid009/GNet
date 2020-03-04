using System;
using GNet.Model;

namespace GNet.Layers.Kernels
{
    [Serializable]
    public class Filter : IKernel
    {
        public IInitializer WeightInit { get; }
        public ShapedArrayImmutable<double> Weights { get; private set; }
        public Shape Shape { get; }
        public bool IsTrainable { get; set; } = true;

        public Filter(Shape shape, IInitializer weightInit)
        {
            Shape = shape;
            WeightInit = weightInit.Clone();
            Weights = new ShapedArrayImmutable<double>(Shape, () => weightInit.Initialize(Shape.Volume, 1));
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
            return new Filter(Shape, WeightInit)
            {
                Weights = Weights,
                IsTrainable = IsTrainable
            };
        }
    }
}