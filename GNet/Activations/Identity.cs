using System;

namespace GNet.Activations
{
    [Serializable]
    public class Identity : IActivation
    {
        public ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => X);
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => 1.0);
        }
    }
}