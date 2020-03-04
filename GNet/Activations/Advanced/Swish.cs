using System;
using static System.Math;

namespace GNet.Activations.Advanced
{
    [Serializable]
    public class Swish : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X / (Exp(-X) + 1.0));
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X =>
            {
                double exp = Exp(X);
                return exp * (1.0 + exp + X) / Pow(1.0 + exp, 2.0);
            });
        }

        public IActivation Clone()
        {
            return new Swish();
        }
    }
}