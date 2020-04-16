using static System.Math;

namespace GNet.Normalizers
{
    public class ZScore : INormalizer
    {
        private double mean;

        private double sd;

        public void UpdateParams(ImmutableArray<ImmutableShapedArray<double>> dataVector)
        {
            mean = dataVector.Sum(D => D.Avarage()) / dataVector.Length;

            double variance = dataVector.Sum(D => D.Sum(X => (X - mean) * (X - mean)));

            int nVals = dataVector[0].Shape.Volume * dataVector.Length;

            sd = Sqrt((variance + double.Epsilon) / nVals);
        }

        public ImmutableShapedArray<double> Normalize(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => (X - mean) / sd);
        }
    }
}