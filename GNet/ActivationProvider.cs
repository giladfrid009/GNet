using System;
using static System.Math;

namespace GNet
{
    public enum Activations { Identity, BinaryStep, ReLU, AbsReLU, LeakyReLU, ELU, Sigmoid, Tanh, LeCunTanh };

    internal static class ActivationProvider
    {
        public static double ActivateValue(Activations activation, double X)
        {
            switch (activation)
            {
                case Activations.Identity: return X;

                case Activations.BinaryStep: return X < 0 ? 0 : 1;

                case Activations.ReLU: return X < 0 ? 0 : X;

                case Activations.AbsReLU: return X < 0 ? -X : X;

                case Activations.LeakyReLU: return X < 0 ? 0.01 * X : X;

                case Activations.ELU: return X < 0 ? Exp(X) - 1 : X;

                case Activations.Sigmoid: return 1 / (1 + Exp(-X));

                case Activations.Tanh: return Tanh(X);

                case Activations.LeCunTanh: return 1.7159 * Tanh(2 / 3 * X);

                default: throw new ArgumentException("Unsupported activation");
            }
        }

        public static double Derive(Activations activation, double X)
        {
            switch (activation)
            {
                case Activations.Identity: return 1;

                case Activations.BinaryStep: return 0;

                case Activations.ReLU: return X < 0 ? 0 : 1;

                case Activations.AbsReLU: return X < 0 ? -1 : 1;

                case Activations.LeakyReLU: return X < 0 ? 0.01 : 1;

                case Activations.ELU: return X < 0 ? X + 1 : 1;

                case Activations.Sigmoid: return X * (1 - X);

                case Activations.Tanh: return 1 - (X * X);

                case Activations.LeCunTanh: return (2.0 / 3.0) / 1.7159 * (1.7159 - X) * (1.7159 + X);

                default: throw new ArgumentException("Unsupported activation");
            }
        }
    }
}
