using System;

namespace GNet.Model
{
    [Serializable]
    public class Kernel : ICloneable<Kernel>
    {
        public IInitializer WeightInit { get; }
        public ShapedArrayImmutable<double> Weights { get; private set; }
        public Shape Shape { get; }

        public Kernel(Shape shape, IInitializer weightInit)
        {
            Shape = shape;
            WeightInit = weightInit.Clone();

            Weights = new ShapedArrayImmutable<double>(shape, weightInit.Initialize(shape.Volume, 1));
        }

        public void Update(ShapedArrayImmutable<Synapse> inSynapses)
        {
            Weights = inSynapses.Select((S, i) => S.BatchWeight + Weights[i]);
        }

        public Kernel Clone()
        {
            return new Kernel(Shape, WeightInit)
            {
                Weights = Weights
            };
        }
    }
}