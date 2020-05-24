namespace GNet
{
    //todo: apply
    public interface IRegularizer
    {
        double Evaluate(double X);

        double Derivative(double X);
    }
}
