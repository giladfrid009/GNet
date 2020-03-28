using System;

namespace GNet.Activations
{
    [Serializable]
    public class ReLu : IActivation
    {
        public double Slope { get; }

        public ReLu(double slope = 0.0)
        {
            Slope = slope;
        }

        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X < 0.0 ? Slope * X : X);
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X < 0.0 ? Slope : 1.0);
        }
    }
}