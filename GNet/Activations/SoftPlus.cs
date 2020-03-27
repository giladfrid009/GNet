using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SoftPlus : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => Log(1.0 + Exp(X)));
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => 1.0 / (1.0 + Exp(-X)));
        }
    }
}