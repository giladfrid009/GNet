using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SoftPlus : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> vals)
        {
            return vals.Select(X => Log(1.0 + Exp(X)));
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> vals)
        {
            return vals.Select(X => 1.0 / (1.0 + Exp(-X)));
        }
    }
}