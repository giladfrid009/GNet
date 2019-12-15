using GNet.Extensions.Array;
using GNet.Extensions.IArray;
using GNet.Extensions.ShapedArray;
using static System.Math;

namespace GNet.Normalizers
{
    public class DecimalScale : INormalizer
    {
        public bool NormalizeInputs { get; set; }
        public bool NormalizeOutputs { get; set; }

        private double scale = 1;

        public void ExtractParams(Dataset dataset)
        {
            double max = 0;

            if (NormalizeInputs)
            {
                dataset.ForEach(D => max = Max(max, D.Inputs.Max()));
            }

            if (NormalizeOutputs)
            {
                dataset.ForEach(D => max = Max(max, D.Outputs.Max()));
            }

            scale = (int)Log10(max) + 1;
        }

        public ShapedArray<double> Normalize(ShapedArray<double> vals)
        {
            return vals.Select(X => X / scale);
        }

        public INormalizer Clone()
        {
            return new DecimalScale()
            {
                NormalizeInputs = NormalizeInputs,
                NormalizeOutputs = NormalizeOutputs,
                scale = scale
            };
        }
    }
}
