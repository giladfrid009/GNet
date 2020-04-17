namespace GNet
{
    public interface INormalizer
    {
        void UpdateParams<TData>(ImmutableArray<TData> dataVector) where TData : ImmutableArray<double>;

        ImmutableArray<double> Normalize(ImmutableArray<double> vals);
    }
}