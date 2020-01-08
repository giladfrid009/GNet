using System;
using GNet.Extensions.IShapedArray;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class LeCunTanh : IActivation
    {
        public double A { get; } = 1.7159;
        public double B { get; } = 2.0 / 3.0;

        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => A * Math.Tanh(B * X));
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => A * B / Pow(Cosh(B * X), 2.0));
        }

        public IActivation Clone()
        {
            return new LeCunTanh();
        }
    }
}
