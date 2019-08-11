using GNet.Extensions.Generic;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Tanh : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => Math.Tanh(X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => 1.0 / Pow(Cosh(X), 2));
        }

        public IActivation Clone()
        {
            return new Tanh();
        }
    }
}
