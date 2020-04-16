using System;
using static System.Math;

namespace GNet.Activations.Advanced
{
    [Serializable]
    public class ArcTan : IActivation
    {
        public ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => Atan(X));
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => 1.0 / (1.0 + X * X));
        }
    }
}