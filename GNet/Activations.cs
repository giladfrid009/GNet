using System;

namespace GNet
{
    public enum ActivationFunctions { Identity, BinaryStep, ReLU, AbsReLU, LeakyReLU, ELU, Sigmoid, Tanh, LeCunTanh };

    internal static class Activations
    {
        public static double ActivateValue(double X, ActivationFunctions activation)
        {
            switch (activation)
            {
                case ActivationFunctions.Identity: return X;

                case ActivationFunctions.BinaryStep: return X < 0 ? 0 : 1;

                case ActivationFunctions.ReLU: return X < 0 ? 0 : X;

                case ActivationFunctions.AbsReLU: return X < 0 ? -X : X;

                case ActivationFunctions.LeakyReLU: return X < 0 ? 0.01 * X : X;

                case ActivationFunctions.ELU: return X < 0 ? Math.Exp(X) - 1 : X;

                case ActivationFunctions.Sigmoid: return 1 / (1 + Math.Exp(-X));

                case ActivationFunctions.Tanh: return Math.Tanh(X);

                case ActivationFunctions.LeCunTanh: return 1.7159 * Math.Tanh(2 / 3 * X);

                default: throw new ArgumentException("Unsupported activation");
            }
        }

        public static double Derivative(double X, ActivationFunctions activation)
        {
            switch (activation)
            {
                case ActivationFunctions.Identity: return 1;

                case ActivationFunctions.BinaryStep: return 0;

                case ActivationFunctions.ReLU: return X < 0 ? 0 : 1;

                case ActivationFunctions.AbsReLU: return X < 0 ? -1 : 1;

                case ActivationFunctions.LeakyReLU: return X < 0 ? 0.01 : 1;

                case ActivationFunctions.ELU: return X < 0 ? X + 1 : 1;

                case ActivationFunctions.Sigmoid: return X * (1 - X);

                case ActivationFunctions.Tanh: return 1 - (X * X);

                case ActivationFunctions.LeCunTanh: return 2 / (3 * 1.7159) * (1.7159 - X) * (1.7159 + X);

                default: throw new ArgumentException("Unsupported activation");
            }
        }
    }
}
