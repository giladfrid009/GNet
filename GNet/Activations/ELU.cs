using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class ELU : IActivation
    {
        public double Activate(double X)
        {
            return X < 0.0 ? (Exp(X) - 1.0) : X;
        }

        public double Derivative(double X, double Y)
        {
            return X < 0.0 ? Exp(X) : 1.0;
        }
    }
}