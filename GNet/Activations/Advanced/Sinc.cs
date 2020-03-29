using System;
using static System.Math;

namespace GNet.Activations.Advanced
{
    [Serializable]
    public class Sinc : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X != 0.0 ? Sin(X) / (X + double.Epsilon) : 1.0);
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X != 0.0 ? Cos(X) / (X + double.Epsilon) - Sin(X) / (X * X + double.Epsilon) : 0.0);
        }
    }
}