using GNet.Extensions.Array;
using GNet.Extensions.IShapedArray;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SoftClipping : IActivation
    {
        public double Alpha { get; }

        public SoftClipping(double alpha)
        {
            Alpha = alpha;
        }

        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => 1.0 / Alpha * Log((1.0 + Exp(Alpha * X)) / (1.0 + Exp(Alpha * X - Alpha))));
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => 0.5 * Sinh(0.5 * Alpha) * (1.0 / Cosh(0.5 * Alpha * X)) * (1.0 / Cosh(0.5 * Alpha * (1.0 - X))));
        }

        public IActivation Clone()
        {
            return new SoftClipping(Alpha);
        }
    }
}
