using System;

namespace GNet.Activations
{
    [Serializable]
    public class HardSigmoid : IActivation
    {
        public double Activate(double X)
        {
            return X < -2.5 ? 0.0 : X > 2.5 ? 1.0 : 0.2 * X + 0.5;
        }

        public double Derivative(double X, double Y)
        {
            return X < -2.5 || X > 2.5 ? 0.0 : 0.2;
        }
    }
}