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

        public ImmutableArray<double> Activate(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => X < 0.0 ? Slope * X : X);
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => X < 0.0 ? Slope : 1.0);
        }
    }
}