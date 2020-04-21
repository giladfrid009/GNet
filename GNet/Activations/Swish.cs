using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Swish : IActivation
    {
        public ImmutableArray<double> Activate(ImmutableArray<double> inputs)
        {
            return inputs.Select(X => X / (Exp(-X) + 1.0));
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> inputs)
        {
            return inputs.Select(X =>
            {
                double exp = Exp(X);
                return exp * (1.0 + exp + X) / Pow(1.0 + exp, 2.0);
            });
        }
    }
}