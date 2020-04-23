using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Softmax : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> inputs)
        {
            ImmutableArray<double> exps = inputs.Select(X => Exp(X));

            double sum = exps.Sum();

            return exps.Select(E => E / sum);
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> inputs)
        {
            ImmutableArray<double> exps = inputs.Select(X => Exp(X));

            double sum = exps.Sum();

            return exps.Select(E => E * (sum - E) / (sum * sum));
        }
    }
}