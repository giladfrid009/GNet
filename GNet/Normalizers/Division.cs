namespace GNet.Normalizers
{
    public class Division : INormalizer
    {
        public double Divisor { get; }

        public Division(double divisor)
        {
            Divisor = divisor;
        }

        public void UpdateParams<TData>(ImmutableArray<TData> dataVector) where TData : ImmutableArray<double>
        {
        }

        public ImmutableArray<double> Normalize(ImmutableArray<double> vals)
        {
            return vals.Select(X => X / Divisor);
        }
    }
}