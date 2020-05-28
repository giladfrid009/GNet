namespace GNet
{
    public interface IRegularizer
    {
        //todo: not used when calcing error. use
        //todo: implement elagantly
        double Evaluate(double X);

        double Derivative(double X);
    }
}
