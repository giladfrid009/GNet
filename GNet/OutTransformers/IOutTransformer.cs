namespace GNet
{
    public interface IOutTransformer
    {
        ShapedArrayImmutable<double> Transform(ShapedArrayImmutable<double> output);
    }
}