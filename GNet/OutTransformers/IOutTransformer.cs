namespace GNet
{
    public interface IOutTransformer
    {
        ImmutableShapedArray<double> Transform(ImmutableShapedArray<double> output);
    }
}