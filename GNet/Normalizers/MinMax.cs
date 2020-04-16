using static System.Math;

namespace GNet.Normalizers
{
    public class MinMax : INormalizer
    {
        private double max = 0;

        private double min = 0;

        public void UpdateParams(ImmutableArray<ImmutableShapedArray<double>> dataVector)
        {
            max = 0;
            min = 0;

            dataVector.ForEach(D => max = Max(max, D.Max()));
            dataVector.ForEach(D => min = Min(min, D.Min()));
        }

        public ImmutableShapedArray<double> Normalize(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => (X - min) / (max - min));
        }
    }
}