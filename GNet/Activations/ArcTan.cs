using GNet.Extensions.Generic;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class ArcTan : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => Atan(X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => 1.0 / (1.0 + X * X));
        }

        public IActivation Clone()
        {
            return new ArcTan();
        }
    }
}
