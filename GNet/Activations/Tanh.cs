using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Tanh : IActivation
    {
        public ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => Tanh(X));
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => 1.0 / Pow(Cosh(X), 2));
        }
    }
}