namespace GNet
{
    public interface IActivation : ICloneable<IActivation>
    {
        double[] Activate(double[] vals);

        double[] Derivative(double[] vals);
    }
}
