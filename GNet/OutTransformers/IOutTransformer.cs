namespace GNet
{
    public interface IOutTransformer
    {
        ImmutableArray<double> Transform(ImmutableArray<double> output);
    }
}