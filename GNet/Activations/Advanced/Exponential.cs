using System;
using static System.Math;

namespace GNet.Activations.Advanced
{
    [Serializable]
    public class Exponential : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => Exp(X));
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => Exp(X));
        }
    }
}