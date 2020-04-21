using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Sigmoid : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => 1.0 / (1.0 + Exp(-X)));
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => Exp(X) / Pow(Exp(X) + 1.0, 2.0));
        }
    }
}