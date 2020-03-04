namespace GNet
{
    public interface ILoss : ICloneable<ILoss>
    {
        double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs);

        ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs);
    }
}