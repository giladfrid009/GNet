using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Sinc : IActivation
    {
        public ShapedArray<double> Activate(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => X != 0.0 ? Sin(X) / X : 1.0);
        }

        public ShapedArray<double> Derivative(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => X != 0.0 ? Cos(X) / X - Sin(X) / (X * X) : 0.0);
        }

        public IActivation Clone()
        {
            return new Sinc();
        }
    }
}
