using static System.Math;

namespace GNet.Normalizers
{
    public class MinMax : INormalizer
    {        
        private double max = 0;
        private double min = 0;

        public void ExtractParams(ArrayImmutable<ShapedArrayImmutable<double>> dataVector)
        {
            max = 0;
            min = 0;

            dataVector.ForEach(D => max = Max(max, D.Max()));
            dataVector.ForEach(D => min = Min(min, D.Min()));
        }

        public ShapedArrayImmutable<double> Normalize(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => (X - min) / (max - min));
        }

        public INormalizer Clone()
        {
            return new MinMax()
            {
                max = max,
                min = min
            };
        }
    }
}