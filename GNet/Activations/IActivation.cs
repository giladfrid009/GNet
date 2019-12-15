namespace GNet
{
    public interface IActivation : ICloneable<IActivation>
    {
        ShapedReadOnlyArray<double> Activate(ShapedReadOnlyArray<double> vals);

        ShapedReadOnlyArray<double> Derivative(ShapedReadOnlyArray<double> vals);
    }
}
