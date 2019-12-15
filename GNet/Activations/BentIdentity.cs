using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class BentIdentity : IActivation
    {
        public ShapedArray<double> Activate(ShapedArray<double> vals)
        {
            return vals.Select(X => (Sqrt(X * X + 1.0) - 1.0) / 2.0 + X);
        }

        public ShapedArray<double> Derivative(ShapedArray<double> vals)
        {
            return vals.Select(X => X / (2.0 * Sqrt(X * X + 1.0)) + 1.0);
        }

        public IActivation Clone()
        {
            return new BentIdentity();
        }
    }
}
