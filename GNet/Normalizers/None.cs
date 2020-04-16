namespace GNet.Normalizers
{
    public class None : INormalizer
    {
        public None()
        {
        }

        public void UpdateParams(ImmutableArray<ImmutableShapedArray<double>> dataVector)
        {
        }

        public ImmutableShapedArray<double> Normalize(ImmutableShapedArray<double> vals)
        {
            return vals;
        }
    }
}