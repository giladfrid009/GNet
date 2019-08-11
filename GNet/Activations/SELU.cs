using GNet.Extensions.Generic;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    /// <summary>
    /// Scaled Exponential Linear Unit
    /// </summary>
    public class SELU : IActivation
    {
        public double A { get; } = 1.0507009873554805;
        public double B { get; } = 1.6732632423543772;

        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? A * B * (Exp(X) - 1.0) : A * X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? A * B * Exp(X) : A);
        }

        public IActivation Clone()
        {
            return new SELU();
        }
    }
}
