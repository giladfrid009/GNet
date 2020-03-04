namespace GNet.Normalizers
{
    public class Division : INormalizer
    {
        public double Divisor { get; }
        public bool NormalizeInputs { get; set; }
        public bool NormalizeOutputs { get; set; }

        public Division(double divisor)
        {
            Divisor = divisor;
        }

        public void ExtractParams(Dataset dataset)
        {
        }

        public ShapedArrayImmutable<double> Normalize(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => X / Divisor);
        }

        public INormalizer Clone()
        {
            return new Division(Divisor);
        }
    }
}