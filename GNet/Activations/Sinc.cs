using GNet.Extensions.IShapedArray;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Sinc : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X != 0.0 ? Sin(X) / X : 1.0);
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X != 0.0 ? Cos(X) / X - Sin(X) / (X * X) : 0.0);
        }

        public IActivation Clone()
        {
            return new Sinc();
        }
    }
}
