using static System.Math;

namespace GNet.Normalizers
{
    public class DecimalScale : INormalizer
    {
        private double scale = 1;

        public void ExtractParams(ArrayImmutable<ShapedArrayImmutable<double>> dataVector)
        {
            double max = 0;

            dataVector.ForEach(D => max = Max(max, D.Max()));

            scale = (int)Log10(max) + 1;
        }

        public ShapedArrayImmutable<double> Normalize(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X / scale);
        }
    }
}