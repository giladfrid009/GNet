using GNet.Extensions.Generic;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SoftPlus : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => Log(1.0 + Exp(X)));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => 1.0 / (1.0 + Exp(-X)));
        }

        public IActivation Clone()
        {
            return new SoftPlus();
        }
    }
}
