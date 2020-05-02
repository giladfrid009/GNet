using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class SoftSign : IActivation
    {
        public double Activate(double X)
        {
            return X / (1.0 + Abs(X));
        }

        public double Derivative(double X, double Y)
        {
            return 1.0 / Pow(1.0 + Abs(X), 2.0);
        }
    }
}