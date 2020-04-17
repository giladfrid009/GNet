using static System.Math;

namespace GNet.Normalizers
{
    public class ZScore : INormalizer
    {
        private double mean;

        private double sd;

        public void UpdateParams<TData>(ImmutableArray<TData> dataVector) where TData : ImmutableArray<double>
        {
            mean = dataVector.Sum(D => D.Avarage()) / dataVector.Length;

            double variance = dataVector.Sum(D => D.Sum(X => (X - mean) * (X - mean)));

            int nVals = dataVector[0].Length * dataVector.Length;

            sd = Sqrt((variance + double.Epsilon) / nVals);
        }

        public ImmutableArray<double> Normalize(ImmutableArray<double> vals)
        {
            return vals.Select(X => (X - mean) / sd);
        }
    }
}