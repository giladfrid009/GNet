using GNet.Extensions.Array.Generic;
using GNet.Extensions.ShapedArray.Generic;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SoftSign : IActivation
    {
        public ShapedArray<double> Activate(ShapedArray<double> vals)
        {
            return vals.Select(X => X / (1.0 + Abs(X)));
        }

        public ShapedArray<double> Derivative(ShapedArray<double> vals)
        {
            return vals.Select(X => 1.0 / Pow(1.0 + Abs(X), 2));
        }

        public IActivation Clone()
        {
            return new SoftSign();
        }
    }
}
