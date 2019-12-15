namespace GNet
{
    public interface IOutTransformer : ICloneable<IOutTransformer>
    {
        ShapedArray<double> Transform(ShapedArray<double> output);
    }
}
