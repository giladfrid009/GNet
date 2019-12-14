using GNet.Extensions.Array.Generic;
using GNet.Extensions.ShapedArray.Generic;
using System;

namespace GNet.Activations
{
    [Serializable]
    public class Identity : IActivation
    {
        public ShapedArray<double> Activate(ShapedArray<double> vals)
        {
            return vals.Select(X => X);
        }

        public ShapedArray<double> Derivative(ShapedArray<double> vals)
        {
            return vals.Select(X => 1.0);
        }

        public IActivation Clone()
        {
            return new Identity();
        }
    }
}
