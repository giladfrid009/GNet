namespace GNet
{
    public interface IActivation : ICloneable<IActivation>
    {
        ShapedArray<double> Activate(ShapedReadOnlyArray<double> vals);

        ShapedArray<double> Derivative(ShapedReadOnlyArray<double> vals);
    }
}
