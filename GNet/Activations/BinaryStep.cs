using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using System;

namespace GNet.Activations
{
    [Serializable]
    public class BinaryStep : IActivation
    {
        public ShapedReadOnlyArray<double> Activate(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => X > 0.0 ? 1.0 : 0.0);
        }

        public ShapedReadOnlyArray<double> Derivative(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => 0.0);
        }

        public IActivation Clone()
        {
            return new BinaryStep();
        }
    }
}
