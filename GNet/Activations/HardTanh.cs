namespace GNet.Activations
{
    public class HardTanh : IActivation
    {
        public double Activate(double X)
        {
            return X < -1.0 ? -1 : X > 1.0 ? 1.0 : X;
        }

        public double Derivative(double X, double Y)
        {
            return X < -1.0 || X > 1.0 ? 0.0 : 1.0;
        }
    }
}