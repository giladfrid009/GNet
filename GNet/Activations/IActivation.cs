namespace GNet
{
    public interface IActivation : ICloneable<IActivation>
    {
        ShapedArray<double> Activate(ShapedArray<double> vals);

        ShapedArray<double> Derivative(ShapedArray<double> vals);
    }
}
