using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Sinusoid : IActivation
    {
        public ShapedReadOnlyArray<double> Activate(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => Sin(X));
        }

        public ShapedReadOnlyArray<double> Derivative(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => Cos(X));
        }

        public IActivation Clone()
        {
            return new Sinusoid();
        }
    }
}
