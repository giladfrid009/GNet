namespace GNet
{
    public interface INormalizer
    {
        void UpdateParams(ImmutableArray<ImmutableShapedArray<double>> dataVector);

        ImmutableShapedArray<double> Normalize(ImmutableShapedArray<double> vals);
    }
}