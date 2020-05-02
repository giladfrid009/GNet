using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Loggy : IActivation
    {
        public double Activate(double X)
        {
            return Tanh(X / 2.0);
        }

        public double Derivative(double X, double Y)
        {
            return 1.0 / (Cosh(X) + 1.0);
        }
    }
}