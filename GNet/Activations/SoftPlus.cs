using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SoftPlus : IActivation
    {
        public double Activate(double X)
        {
            return Log(1.0 + Exp(X));
        }

        public double Derivative(double X, double Y)
        {
            return 1.0 / (1.0 + Exp(-X));
        }
    }
}