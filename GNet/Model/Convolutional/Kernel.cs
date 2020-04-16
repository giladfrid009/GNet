using System;

namespace GNet.Model.Convolutional
{
    [Serializable]
    public class Kernel
    {
        public SharedVal<double> Bias { get; }
        public ImmutableShapedArray<SharedVal<double>> Weights { get; }
        public Shape Shape { get; }

        public Kernel(Shape shape)
        {
            Shape = shape;

            Bias = new SharedVal<double>();
            Weights = new ImmutableShapedArray<SharedVal<double>>(shape, () => new SharedVal<double>());
        }
    }
}