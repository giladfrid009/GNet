namespace GNet
{
    public interface IActivation
    {
        ImmutableShapedArray<double> Activate(ImmutableShapedArray<double> vals);

        ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> vals);
    }
}