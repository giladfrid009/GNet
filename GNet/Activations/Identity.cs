using System;

namespace GNet.Activations
{
    [Serializable]
    public class Identity : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> vals)
        {
            return vals.Select(X => X);
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> vals)
        {
            return vals.Select(X => 1.0);
        }
    }
}