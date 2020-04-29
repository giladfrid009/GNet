using static System.Math;

namespace GNet.Normalizers
{
    public class MinMax : INormalizer
    {
        public double Min { get; private set; }
        public double Max { get; private set; }

        public void UpdateParams<TData>(ImmutableArray<TData> dataVector) where TData : ImmutableArray<double>
        {
            Min = dataVector.Min(D => D.Min());
            Max = dataVector.Max(D => D.Max());

            if(Min == Max)
            {
                Max += double.Epsilon;
            }
        }

        public ImmutableArray<double> Normalize(ImmutableArray<double> vals)
        {
            return vals.Select(X => (X - Min) / (Max - Min));
        }
    }
}