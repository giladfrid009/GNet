using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SoftSign : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => X / (1.0 + Abs(X)));
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => 1.0 / Pow(1.0 + Abs(X), 2.0));
        }
    }
}