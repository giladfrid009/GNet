namespace GNet
{
    public interface ILoss : ICloneable<ILoss>
    {
        double Compute(ShapedArray<double> targets, ShapedArray<double> outputs);

        ShapedArray<double> Derivative(ShapedArray<double> targets, ShapedArray<double> outputs);
    }
}
