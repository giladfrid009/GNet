using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Tanh : IActivation
    {
        public ShapedArray<double> Activate(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => Math.Tanh(X));
        }

        public ShapedArray<double> Derivative(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => 1.0 / Pow(Cosh(X), 2));
        }

        public IActivation Clone()
        {
            return new Tanh();
        }
    }
}
