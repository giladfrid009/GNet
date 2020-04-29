using static System.Math;

namespace GNet.Normalizers
{
    public class ZScore : INormalizer
    {
        public double Avg { get; private set; }
        public double SD { get; private set; }

        public void UpdateParams<TData>(ImmutableArray<TData> dataVector) where TData : ImmutableArray<double>
        {
            Avg = dataVector.Avarage(D => D.Avarage());

            double var = dataVector.Sum(D => D.Sum(X => (X - Avg) * (X - Avg)));

            int nVals = dataVector[0].Length * dataVector.Length;

            SD = Sqrt((var + double.Epsilon) / nVals);
        }

        public ImmutableArray<double> Normalize(ImmutableArray<double> vals)
        {
            return vals.Select(X => (X - Avg) / SD);
        }
    }
}