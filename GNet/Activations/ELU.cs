using GNet.Extensions.Generic;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    /// <summary>
    /// Exponential Linear Unit
    /// </summary>
    public class ELU : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? (Exp(X) - 1.0) : X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? Exp(X) : 1.0);
        }

        public IActivation Clone()
        {
            return new ELU();
        }
    }
}
