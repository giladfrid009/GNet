using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class ELU : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X < 0.0 ? (Exp(X) - 1.0) : X);
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X < 0.0 ? Exp(X) : 1.0);
        }
    }
}