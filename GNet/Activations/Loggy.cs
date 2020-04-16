using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Loggy : IActivation
    {
        public ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => Tanh(X / 2.0));
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => 1.0 / (Cosh(X) + 1.0));
        }
    }
}