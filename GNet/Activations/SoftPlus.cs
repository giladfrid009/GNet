using GNet.Extensions.Array.Generic;
using GNet.Extensions.ShapedArray.Generic;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SoftPlus : IActivation
    {
        public ShapedArray<double> Activate(ShapedArray<double> vals)
        {
            return vals.Select(X => Log(1.0 + Exp(X)));
        }

        public ShapedArray<double> Derivative(ShapedArray<double> vals)
        {
            return vals.Select(X => 1.0 / (1.0 + Exp(-X)));
        }

        public IActivation Clone()
        {
            return new SoftPlus();
        }
    }
}
