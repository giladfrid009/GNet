using System;

namespace GNet.Activations
{
    [Serializable]
    public class Identity : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => X);
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => 1.0);
        }
    }
}