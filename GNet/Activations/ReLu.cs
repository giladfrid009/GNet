using System;

namespace GNet.Activations
{
    [Serializable]
    public class Relu : IActivation
    {
        public double Slope { get; }

        public Relu(double slope = 0.0)
        {
            Slope = slope;
        }

        public double Activate(double X)
        {
            return X < 0.0 ? Slope * X : X;
        }

        public double Derivative(double X, double Y)
        {
            return X < 0.0 ? Slope : 1.0;
        }
    }
}