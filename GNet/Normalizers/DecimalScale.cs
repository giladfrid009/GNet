using static System.Math;

namespace GNet.Normalizers
{
    public class DecimalScale : INormalizer
    {
        public double Scale { get; private set; }

        public void UpdateParams<TData>(ImmutableArray<TData> dataVector) where TData : ImmutableArray<double>
        {
            double max = dataVector.Max(D => D.Max(X => Abs(X)));

            Scale = (int)Log10(max) + 1;
        }

        public ImmutableArray<double> Normalize(ImmutableArray<double> vals)
        {
            return vals.Select(X => X / Scale);
        }
    }
}