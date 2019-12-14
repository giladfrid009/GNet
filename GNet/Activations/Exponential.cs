using GNet.Extensions.Array.Generic;
using GNet.Extensions.ShapedArray.Generic;
using static System.Math;


namespace GNet.Activations
{
    public class Exponential : IActivation
    {
        public ShapedArray<double> Activate(ShapedArray<double> vals)
        {
            return vals.Select(X => Exp(X));
        }

        public ShapedArray<double> Derivative(ShapedArray<double> vals)
        {
            return vals.Select(X => Exp(X));
        }

        public IActivation Clone()
        {
            return new Exponential();
        }
    }
}
