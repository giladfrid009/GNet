using GNet.Extensions.Array;
using GNet.Extensions.IArray;
using GNet.Extensions.IShapedArray;
using static System.Math;

namespace GNet.Normalizers
{
    public class MinMax : INormalizer
    {
        public bool NormalizeInputs { get; set; }
        public bool NormalizeOutputs { get; set; }

        private double max = 0;
        private double min = 0;

        public void ExtractParams(Dataset dataset)
        {
            max = 0;
            min = 0;

            if (NormalizeInputs)
            {
                dataset.ForEach(D => max = Max(max, D.Inputs.Max()));
                dataset.ForEach(D => min = Min(min, D.Inputs.Min()));
            }

            if (NormalizeOutputs)
            {
                dataset.ForEach(D => max = Max(max, D.Outputs.Max()));
                dataset.ForEach(D => min = Min(min, D.Inputs.Min()));
            }
        }

        public ShapedArrayImmutable<double> Normalize(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => (X - min) / (max - min));
        }

        public INormalizer Clone()
        {
            return new MinMax()
            {
                NormalizeInputs = NormalizeInputs,
                NormalizeOutputs = NormalizeOutputs,
                max = max,
                min = min
            };
        }
    }
}
