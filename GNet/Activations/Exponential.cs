using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class Exponential : IActivation
    {
        public double Activate(double X)
        {
            return Exp(X);
        }

        public double Derivative(double X, double Y)
        {
            return Y;
        }
    }
}