using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using System;

namespace GNet.Activations
{
    [Serializable]
    public class HardSigmoid : IActivation
    {
        public ShapedReadOnlyArray<double> Activate(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => X < -2.5 ? 0.0 : X > 2.5 ? 1.0 : 0.2 * X + 0.5);
        }

        public ShapedReadOnlyArray<double> Derivative(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => X < -2.5 || X > 2.5 ? 0.0 : 0.2);
        }

        public IActivation Clone()
        {
            return new HardSigmoid();
        }
    }
}
