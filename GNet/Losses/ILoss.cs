namespace GNet
{
    public interface ILoss : ICloneable<ILoss>
    {
        double Compute(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs);

        ShapedArray<double> Derivative(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs);
    }
}
