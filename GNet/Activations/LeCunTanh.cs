using GNet.Extensions.Array.Generic;
using GNet.Extensions.ShapedArray.Generic;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class LeCunTanh : IActivation
    {
        public double A { get; } = 1.7159;
        public double B { get; } = 2.0 / 3.0;

        public ShapedArray<double> Activate(ShapedArray<double> vals)
        {
            return vals.Select(X => A * Math.Tanh(B * X));
        }

        public ShapedArray<double> Derivative(ShapedArray<double> vals)
        {
            return vals.Select(X => A * B / Pow(Cosh(B * X), 2.0));
        }

        public IActivation Clone()
        {
            return new LeCunTanh();
        }
    }
}
