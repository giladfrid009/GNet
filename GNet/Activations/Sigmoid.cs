using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Sigmoid : IActivation
    {
        public double Activate(double X)
        {
            return 1.0 / (1.0 + Exp(-X));
        }

        public double Derivative(double X, double Y)
        {
            return Y * (1.0 - Y);
        }
    }
}