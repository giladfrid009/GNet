using System;
using static System.Math;

namespace GNet.Activations.Advanced
{
    [Serializable]
    public class Sinusoid : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => Sin(X));
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => Cos(X));
        }
    }
}