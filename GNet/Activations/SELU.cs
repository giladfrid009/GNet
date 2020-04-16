using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SELU : IActivation
    {
        private readonly double a = 1.0507009873554805;
        private readonly double b = 1.6732632423543772;

        public ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => X < 0.0 ? a * b * (Exp(X) - 1.0) : a * X);
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => X < 0.0 ? a * b * Exp(X) : a);
        }
    }
}