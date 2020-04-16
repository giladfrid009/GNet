using System;

namespace GNet.Activations
{
    [Serializable]
    public class HardSigmoid : IActivation
    {
        public ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => X < -2.5 ? 0.0 : X > 2.5 ? 1.0 : 0.2 * X + 0.5);
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => X < -2.5 || X > 2.5 ? 0.0 : 0.2);
        }
    }
}