using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Exponential : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => Exp(X));
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => Exp(X));
        }
    }
}