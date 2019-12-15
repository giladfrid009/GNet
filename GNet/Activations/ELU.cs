using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
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
        public ShapedReadOnlyArray<double> Activate(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => X < 0.0 ? (Exp(X) - 1.0) : X);
        }

        public ShapedReadOnlyArray<double> Derivative(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => X < 0.0 ? Exp(X) : 1.0);
        }

        public IActivation Clone()
        {
            return new ELU();
        }
    }
}
