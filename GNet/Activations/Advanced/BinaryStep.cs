using System;

namespace GNet.Activations.Advanced
{
    [Serializable]
    public class BinaryStep : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X > 0.0 ? 1.0 : 0.0);
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => 0.0);
        }

        public IActivation Clone()
        {
            return new BinaryStep();
        }
    }
}