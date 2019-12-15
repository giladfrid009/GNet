using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using System;

namespace GNet.Activations
{
    [Serializable]
    public class Identity : IActivation
    {
        public ShapedArray<double> Activate(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => X);
        }

        public ShapedArray<double> Derivative(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => 1.0);
        }

        public IActivation Clone()
        {
            return new Identity();
        }
    }
}
