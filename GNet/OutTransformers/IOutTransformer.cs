namespace GNet
{
    public interface IOutTransformer : ICloneable<IOutTransformer>
    {
        ShapedArray<double> Transform(ShapedReadOnlyArray<double> output);
    }
}
