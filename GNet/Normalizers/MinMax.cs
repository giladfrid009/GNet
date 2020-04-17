using static System.Math;

namespace GNet.Normalizers
{
    public class MinMax : INormalizer
    {
        private double max = 0;

        private double min = 0;

        public void UpdateParams<TData>(ImmutableArray<TData> dataVector) where TData : ImmutableArray<double>
        {
            max = 0;
            min = 0;

            dataVector.ForEach(D => max = Max(max, D.Max()));
            dataVector.ForEach(D => min = Min(min, D.Min()));
        }

        public ImmutableArray<double> Normalize(ImmutableArray<double> vals)
        {
            return vals.Select(X => (X - min) / (max - min));
        }
    }
}