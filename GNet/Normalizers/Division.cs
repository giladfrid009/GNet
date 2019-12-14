using GNet.Extensions.Array.Generic;
using GNet.Extensions.ShapedArray.Generic;
namespace GNet.Normalizers
{
    public class Division : INormalizer
    {
        public bool NormalizeInputs { get; set; }
        public bool NormalizeOutputs { get; set; }
        public double Divisor { get; }

        public Division(double divisor)
        {
            Divisor = divisor;
        }

        public void ExtractParams(Dataset dataset) { }

        public ShapedArray<double> Normalize(ShapedArray<double> vals)
        {
            return vals.Select(X => X / Divisor);
        }

        public INormalizer Clone()
        {
            return new Division(Divisor);
        }
    }
}
