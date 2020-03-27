namespace GNet
{
    public interface IActivation
    {
        ShapedArrayImmutable<double> Activate(ShapedArrayImmutable<double> vals);

        ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals);
    }
}