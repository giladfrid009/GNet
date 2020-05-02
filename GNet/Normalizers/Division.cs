namespace GNet.Normalizers
{
    public class Division : INormalizer
    {
        public double Divisor { get; }

        public Division(double divisor)
        {
            Divisor = divisor;
        }

        public void UpdateParams(Dataset dataset, bool inputs, bool targets)
        {
        }

        public double Normalize(double X)
        {
            return X / Divisor;
        }
    }
}