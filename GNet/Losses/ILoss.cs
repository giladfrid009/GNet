namespace GNet
{
    public interface ILoss : ICloneable<ILoss>
    {
        double Compute(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs);

        ShapedReadOnlyArray<double> Derivative(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs);
    }
}
