using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Gaussian : IActivation
    {
        public ShapedArray<double> Activate(ShapedArray<double> vals)
        {
            return vals.Select(X => Exp(-X * X));
        }

        public ShapedArray<double> Derivative(ShapedArray<double> vals)
        {
            return vals.Select(X => -2.0 * X * Exp(-X * X));
        }

        public IActivation Clone()
        {
            return new Gaussian();
        }
    }
}
