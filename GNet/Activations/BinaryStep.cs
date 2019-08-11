using GNet.Extensions.Generic;
using System;

namespace GNet.Activations
{
    [Serializable]
    public class BinaryStep : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X > 0.0 ? 1.0 : 0.0);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => 0.0);
        }

        public IActivation Clone()
        {
            return new BinaryStep();
        }
    }
}
