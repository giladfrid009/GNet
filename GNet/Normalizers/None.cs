namespace GNet.Normalizers
{
    public class None : INormalizer
    {
        public None()
        {
        }

        public void UpdateParams<TData>(ImmutableArray<TData> dataVector) where TData : ImmutableArray<double>
        {
        }

        public ImmutableArray<double> Normalize(ImmutableArray<double> vals)
        {
            return vals;
        }
    }
}