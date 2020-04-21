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

        //todo: fix derivative. it should be wrt to  everyone cuz loss of single neuron is dependent on all the neurons
        public ImmutableArray<double> Derivative(ImmutableArray<double> inputs)
        {
            ImmutableArray<double> exps = inputs.Select(X => Exp(X));

            double sum = exps.Sum();

            return exps.Select(E => E / sum * (1.0 - E / sum));
        }
    }
}