using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Loggy : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> vals)
        {
            return vals.Select(X => Tanh(X / 2.0));
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> vals)
        {
            return vals.Select(X => 1.0 / (Cosh(X) + 1.0));
        }
    }
}