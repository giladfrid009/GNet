using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Tanh : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> vals)
        {
            return vals.Select(X => Tanh(X));
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> vals)
        {
            return vals.Select(X => 1.0 / Pow(Cosh(X), 2));
        }
    }
}