namespace GNet
{
    public interface IActivation
    {
        ImmutableArray<double> Activate(ImmutableArray<double> inputs);

        ImmutableArray<double> Derivative(ImmutableArray<double> inputs);
    }
}