namespace GNet
{
    public interface ILoss
    {
        double Compute(ImmutableArray<double> targets, ImmutableArray<double> outputs);

        ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs);
    }
}