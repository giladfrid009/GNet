using GNet.Extensions.IShapedArray;
using System;

namespace GNet.Activations
{
    [Serializable]
    public class Identity : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X);
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => 1.0);
        }

        public IActivation Clone()
        {
            return new Identity();
        }
    }
}
