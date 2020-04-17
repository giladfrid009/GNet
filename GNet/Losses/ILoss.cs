namespace GNet
{
    public interface ILoss
    {
        //toso: sort losses into categorical and binary
        double Compute(ImmutableArray<double> targets, ImmutableArray<double> outputs);

        ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs);
    }
}