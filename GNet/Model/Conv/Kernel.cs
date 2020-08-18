using NCollections;
using System;

namespace GNet.Model.Conv
{
    [Serializable]
    public class Kernel
    {
        public Array<SharedVal<double>> Weights { get; }
        public SharedVal<double> Bias { get; }
        public Shape Shape { get; }

        public Kernel(Shape shape)
        {
            Shape = shape;

            Bias = new SharedVal<double>();
            Weights = new Array<SharedVal<double>>(shape.Volume, () => new SharedVal<double>());
        }
    }
}