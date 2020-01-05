using GNet.Extensions.IShapedArray;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    /// <summary>
    /// Exponential Linear Unit
    /// </summary>
    public class ELU : IActivation
    {
        public ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X < 0.0 ? (Exp(X) - 1.0) : X);
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X < 0.0 ? Exp(X) : 1.0);
        }

        public IActivation Clone()
        {
            return new ELU();
        }
    }
}
