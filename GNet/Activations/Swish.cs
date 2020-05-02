using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Swish : IActivation
    {
        public double Activate(double X)
        {
            return X / (1.0 + Exp(-X));
        }

        public double Derivative(double X, double Y)
        {
            double E = Exp(X);
            return E * (1.0 + E + X) / Pow(1.0 + E, 2.0);
        }
    }
}