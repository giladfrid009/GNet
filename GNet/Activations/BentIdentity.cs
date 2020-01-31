using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class BentIdentity : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => (Sqrt(X * X + 1.0) - 1.0) / 2.0 + X);
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X / (2.0 * Sqrt(X * X + 1.0)) + 1.0);
        }

        public IActivation Clone()
        {
            return new BentIdentity();
        }
    }
}
