using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Exponential : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> vals)
        {
            return vals.Select(X => Exp(X));
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> vals)
        {
            return vals.Select(X => Exp(X));
        }
    }
}