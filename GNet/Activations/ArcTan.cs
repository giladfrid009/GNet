using System;
using static System.Math;

namespace GNet.Activations.Advanced
{
    [Serializable]
    public class ArcTan : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> vals)
        {
            return vals.Select(X => Atan(X));
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> vals)
        {
            return vals.Select(X => 1.0 / (1.0 + X * X));
        }
    }
}