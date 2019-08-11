using GNet.Extensions.Generic;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Sigmoid : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => 1.0 / (1.0 + Exp(-X)));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => Exp(X) / Pow(Exp(X) + 1.0, 2.0));
        }

        public IActivation Clone()
        {
            return new Sigmoid();
        }
    }
}
