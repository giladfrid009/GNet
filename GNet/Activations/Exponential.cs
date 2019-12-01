using GNet.Extensions.Generic;
using static System.Math;


namespace GNet.Activations
{
    public class Exponential : IActivation
    {
        public double[] Activate(double[] vals)
        {
            return vals.Select(X => Exp(X));
        }

        public double[] Derivative(double[] vals)
        {
            return vals.Select(X => Exp(X));
        }

        public IActivation Clone()
        {
            return new Exponential();
        }
    }
}
