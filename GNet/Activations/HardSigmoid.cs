using GNet.Extensions.Generic;
using System;

namespace GNet.Activations
{
    [Serializable]
    public class HardSigmoid : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X < -2.5 ? 0.0 : X > 2.5 ? 1.0 : 0.2 * X + 0.5);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => X < -2.5 || X > 2.5 ? 0.0 : 0.2);
        }

        public IActivation Clone()
        {
            return new HardSigmoid();
        }
    }
}
