namespace GNet
{
    //todo: implement
    public interface IRegularizer
    {
        double Evaluate(double X);

        double Derivative(double X);
    }
}
