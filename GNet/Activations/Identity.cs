using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using System;

namespace GNet.Activations
{
    [Serializable]
    public class Identity : IActivation
    {
        public ShapedReadOnlyArray<double> Activate(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => X);
        }

        public ShapedReadOnlyArray<double> Derivative(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => 1.0);
        }

        public IActivation Clone()
        {
            return new Identity();
        }
    }
}
