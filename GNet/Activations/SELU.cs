using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SELU : IActivation
    {
        private readonly double a = 1.0507009873554805;
        private readonly double b = 1.6732632423543772;

        public ImmutableArray<double> Activate(ImmutableArray<double> vals)
        {
            return vals.Select(X => X < 0.0 ? a * b * (Exp(X) - 1.0) : a * X);
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> vals)
        {
            return vals.Select(X => X < 0.0 ? a * b * Exp(X) : a);
        }
    }
}