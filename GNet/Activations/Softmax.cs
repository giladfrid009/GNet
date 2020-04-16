using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Softmax : IActivation
    {
        public ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals)
        {
            ImmutableShapedArray<double> exps = vals.Select(X => Exp(X));

            double sum = exps.Sum();

            return exps.Select(E => E / sum);
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals)
        {
            ImmutableShapedArray<double> exps = vals.Select(X => Exp(X));

            double sum = exps.Sum();

            return exps.Select(E => E * (sum - E) / (sum * sum));
        }
    }
}