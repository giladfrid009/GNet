using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Tanh : IActivation
    {
        public double Activate(double X)
        {
            return Tanh(X);
        }

        public double Derivative(double X, double Y)
        {
            return 1.0 - Y * Y;
        }
    }
}