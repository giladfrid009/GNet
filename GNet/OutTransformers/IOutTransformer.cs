namespace GNet
{
    public interface IOutTransformer : ICloneable<IOutTransformer>
    {
        ShapedReadOnlyArray<double> Transform(ShapedReadOnlyArray<double> output);
    }
}
