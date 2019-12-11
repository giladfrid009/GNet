using GNet.Extensions.Generic;
using GNet.Extensions.Math;
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
            double maxInput = 0;
            double maxOutput = 0;

            if (NormalizeInputs)
            {
                maxInput = dataset.DataCollection.Select(D => D.Inputs.Select(X => Abs(X)).Max()).Max();
            }

            if (NormalizeOutputs)
            {
                maxOutput = dataset.DataCollection.Select(D => D.Outputs.Select(X => Abs(X)).Max()).Max();
            }

            double max = Max(maxInput, maxOutput);

            scale = (int)Log10(max) + 1;
        }

        public double[] Normalize(double[] vals)
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
