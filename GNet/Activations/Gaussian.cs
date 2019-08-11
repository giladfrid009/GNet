using GNet.Extensions.Generic;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Gaussian : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => Exp(-X * X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => -2.0 * X * Exp(-X * X));
        }

        public IActivation Clone()
        {
            return new Gaussian();
        }
    }
}
