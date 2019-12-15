using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using static System.Math;


namespace GNet.Activations
{
    public class Exponential : IActivation
    {
        public ShapedReadOnlyArray<double> Activate(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => Exp(X));
        }

        public ShapedReadOnlyArray<double> Derivative(ShapedReadOnlyArray<double> vals)
        {
            return vals.Select(X => Exp(X));
        }

        public IActivation Clone()
        {
            return new Exponential();
        }
    }
}
