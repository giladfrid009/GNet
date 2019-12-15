using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class LeCunTanh : IActivation
    {
        public double A { get; } = 1.7159;
        public double B { get; } = 2.0 / 3.0;

        public ShapedArray<double> Activate(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => A * Math.Tanh(B * X));
        }

        public ShapedArray<double> Derivative(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => A * B / Pow(Cosh(B * X), 2.0));
        }

        public IActivation Clone()
        {
            return new LeCunTanh();
        }
    }
}
