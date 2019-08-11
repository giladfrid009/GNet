using GNet.Extensions.Generic;
using System;

namespace GNet.Activations
{
    [Serializable]
    public class Identity : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => 1.0);
        }

        public IActivation Clone()
        {
            return new Identity();
        }
    }
}
