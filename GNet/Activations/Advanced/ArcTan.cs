using System;
using static System.Math;

namespace GNet.Activations.Advanced
{
    [Serializable]
    public class ArcTan : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => Atan(X));
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => 1.0 / (1.0 + X * X));
        }
    }
}