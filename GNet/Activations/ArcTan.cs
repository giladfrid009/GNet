using System;
using static System.Math;

namespace GNet.Activations
{
    [Serializable]
    public class ArcTan : IActivation
    {
        public double Activate(double X)
        {
            return Atan(X);
        }

        public double Derivative(double X, double Y)
        {
            return 1.0 / (1.0 + X * X);
        }
    }
}