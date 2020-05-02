namespace GNet
{
    public interface IActivation
    {
        double Activate(double X);

        double Derivative(double X, double Y);
    }
}