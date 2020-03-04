namespace GNet
{
    public interface IOutTransformer : ICloneable<IOutTransformer>
    {
        ShapedArrayImmutable<double> Transform(ShapedArrayImmutable<double> output);
    }
}