using GNet.Extensions.Generic;
using System;

namespace GNet.Activations
{
    [Serializable]
    /// <summary>
    /// Rectified Linear Unit
    /// </summary>
    public class ReLu : IActivation
    {
        public double Slope { get; }

        public ReLu(double slope = 0.0)
        {
            Slope = slope;
        }

        public double[] Activate(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? Slope * X : X);
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => X < 0.0 ? Slope : 1.0);
        }

        public IActivation Clone()
        {
            return new ReLu(Slope);
        }
    }
}
