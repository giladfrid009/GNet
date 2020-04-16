using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class ELU : IActivation
    {
        public ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => X < 0.0 ? (Exp(X) - 1.0) : X);
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => X < 0.0 ? Exp(X) : 1.0);
        }
    }
}