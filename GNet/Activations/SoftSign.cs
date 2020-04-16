using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SoftSign : IActivation
    {
        public ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => X / (1.0 + Abs(X)));
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => 1.0 / Pow(1.0 + Abs(X), 2));
        }
    }
}