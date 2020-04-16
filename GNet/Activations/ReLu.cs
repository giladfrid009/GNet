using System;

namespace GNet.Activations
{
    [Serializable]
    public class ReLU : IActivation
    {
        public double Slope { get; }

        public ReLU(double slope = 0.0)
        {
            Slope = slope;
        }

        public ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => X < 0.0 ? Slope * X : X);
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => X < 0.0 ? Slope : 1.0);
        }
    }
}