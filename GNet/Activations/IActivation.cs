namespace GNet
{
    public interface IActivation : ICloneable<IActivation>
    {
        ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals);

        ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals);
    }
}
