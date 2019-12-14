using GNet.Extensions.Array.Generic;
using GNet.Extensions.ShapedArray.Generic;
using System;

namespace GNet.Activations
{
    [Serializable]
    public class HardSigmoid : IActivation
    {
        public ShapedArray<double> Activate(ShapedArray<double> vals)
        {
            return vals.Select(X => X < -2.5 ? 0.0 : X > 2.5 ? 1.0 : 0.2 * X + 0.5);
        }

        public ShapedArray<double> Derivative(ShapedArray<double> vals)
        {
            return vals.Select(X => X < -2.5 || X > 2.5 ? 0.0 : 0.2);
        }

        public IActivation Clone()
        {
            return new HardSigmoid();
        }
    }
}
