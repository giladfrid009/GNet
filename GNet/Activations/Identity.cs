using System;

namespace GNet.Activations
{
    [Serializable]
    public class Identity : IActivation
    {
        public double Activate(double X)
        {
            return X;
        }

        public double Derivative(double X, double Y)
        {
            return 1.0;
        }
    }
}