using static System.Math;

namespace GNet.Normalizers
{
    public class ZScore : INormalizer
    {
        private double avg;
        private double sd;

        public void UpdateParams<TData>(ImmutableArray<TData> dataVector) where TData : ImmutableArray<double>
        {
            avg = dataVector.Sum(D => D.Avarage()) / dataVector.Length;

            double var = dataVector.Sum(D => D.Sum(X => (X - avg) * (X - avg)));

            int nVals = dataVector[0].Length * dataVector.Length;

            sd = Sqrt((var + double.Epsilon) / nVals);
        }

        public ImmutableArray<double> Normalize(ImmutableArray<double> vals)
        {
            return vals.Select(X => (X - avg) / sd);
        }
    }
}