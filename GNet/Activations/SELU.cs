using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SELU : IActivation
    {
        private const double A = 1.0507009873554805;
        private const double B = 1.6732632423543772;

        public ImmutableArray<double> Activate(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => X < 0.0 ? A * B * (Exp(X) - 1.0) : A * X);
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => X < 0.0 ? A * B * Exp(X) : A);
        }
    }
}