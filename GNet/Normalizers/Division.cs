namespace GNet.Normalizers
{
    public class Division : INormalizer
    {
        public double Divisor { get; }

        public Division(double divisor)
        {
            Divisor = divisor;
        }

        public void UpdateParams(ImmutableArray<ImmutableShapedArray<double>> dataVector)
        {
        }

        public ImmutableShapedArray<double> Normalize(ImmutableShapedArray<double> vals)
        {
            return vals.Select(X => X / Divisor);
        }
    }
}