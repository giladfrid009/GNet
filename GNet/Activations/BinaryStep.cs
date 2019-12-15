using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using System;

namespace GNet.Activations
{
    [Serializable]
    public class BinaryStep : IActivation
    {
        public ShapedArray<double> Activate(ShapedArray<double> vals)
        {
            return vals.Select(X => X > 0.0 ? 1.0 : 0.0);
        }

        public ShapedArray<double> Derivative(ShapedArray<double> vals)
        {
            return vals.Select(X => 0.0);
        }

        public IActivation Clone()
        {
            return new BinaryStep();
        }
    }
}
