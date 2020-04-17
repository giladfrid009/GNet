namespace GNet
{
    public interface IActivation
    {
        ImmutableArray<double> Activate(ImmutableArray<double> vals);

        ImmutableArray<double> Derivative(ImmutableArray<double> vals);
    }
}