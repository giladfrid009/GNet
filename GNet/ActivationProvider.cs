using System;
using static System.Math;

namespace GNet
{
    public delegate double ActivationFunc(double value);

    public enum Activations { Identity, BinaryStep, ReLU, LeakyReLU, ELU, Sigmoid, HardSigmoid, Tanh, LeCunTanh, Softplus };

    internal static class ActivationProvider
    {
        public static ActivationFunc GetActivation(Activations activation)
        {
            switch (activation)
            {
                case Activations.Identity: return (X) => X;

                case Activations.BinaryStep: return (X) => X < 0 ? 0 : 1;

                case Activations.ReLU: return (X) => X < 0 ? 0 : X;

                case Activations.LeakyReLU: return (X) => X < 0 ? 0.01 * X : X;

                case Activations.ELU: return (X) => X < 0 ? Exp(X) - 1 : X;

                case Activations.Sigmoid: return (X) => 1 / (1 + Exp(-X));

                case Activations.HardSigmoid: return (X) => X < -2.5 ? 0 : X > 2.5 ? 1 : 0.2 * X + 0.5;

                case Activations.Tanh: return (X) => Tanh(X);

                case Activations.LeCunTanh: return (X) => 1.7159 * Tanh(2 / 3 * X);

                case Activations.Softplus: return (X) => Log(1 + Exp(X));

                default: throw new ArgumentException("Unsupported activation");
            }
        }

        public static ActivationFunc GetDerivative(Activations activation)
        {
            switch (activation)
            {
                case Activations.Identity: return (X) => 1;

                case Activations.BinaryStep: return (X) => 0;

                case Activations.ReLU: return (X) => X == 0 ? 0 : 1;

                case Activations.LeakyReLU: return (X) => X < 0 ? 0.01 : 1;

                case Activations.ELU: return (X) => X < 0 ? Exp(X) : 1;

                case Activations.Sigmoid: return (X) => X * (1 - X);

                case Activations.HardSigmoid: return (X) => X == 0 || X == 1 ? 0 : 0.2;

                case Activations.Tanh: return (X) => 1 - (X * X);

                case Activations.LeCunTanh: return (X) => (2.0 / 3.0) / 1.7159 * (1.7159 - X) * (1.7159 + X);

                case Activations.Softplus: return (X) => 1 - (1 / Exp(X));

                default: throw new ArgumentException("Unsupported activation");
            }
        }       
    }
}
