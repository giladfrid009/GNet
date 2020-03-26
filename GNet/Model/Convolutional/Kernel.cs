using System;

namespace GNet.Model.Convolutional
{
    [Serializable]
    public class Kernel : ICloneable<Kernel>
    {
        public SharedVal<double> Bias { get; private set; }
        public ShapedArrayImmutable<SharedVal<double>> Weights { get; private set; }
        public Shape Shape { get; }

        public Kernel(Shape shape)
        {
            Shape = shape;

            Bias = new SharedVal<double>();
            Weights = new ShapedArrayImmutable<SharedVal<double>>(shape, () => new SharedVal<double>());
        }

        public Kernel Clone()
        {
            return new Kernel(Shape)
            {
                Weights = Weights.Select(X => X.Clone()),
                Bias = Bias.Clone()
            };
        }
    }
}