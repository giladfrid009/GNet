using GNet.Extensions.IShapedArray;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Tanh : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => Math.Tanh(X));
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => 1.0 / Pow(Cosh(X), 2));
        }

        public IActivation Clone()
        {
            return new Tanh();
        }
    }
}
