namespace GNet.Normalizers
{
    public class Division : INormalizer
    {
        public double Divisor { get; }

        public Division(double divisor)
        {
            Divisor = divisor;
        }

        public void ExtractParams(ArrayImmutable<ShapedArrayImmutable<double>> dataVector)
        {
        }

        public ShapedArrayImmutable<double> Normalize(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X / Divisor);
        }
    }
}