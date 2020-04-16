using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SoftPlus : IActivation
    {
        public ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => Log(1.0 + Exp(X)));
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => 1.0 / (1.0 + Exp(-X)));
        }
    }
}