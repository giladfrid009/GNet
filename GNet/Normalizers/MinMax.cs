using GNet.Extensions.Generic;
using GNet.Extensions.Math;
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
            double maxInput = 0;
            double maxOutput = 0;
            double minInput = 0;
            double minOutput = 0;

            if (NormalizeInputs)
            {
                maxInput = dataset.DataCollection.Select(D => D.Inputs.Max()).Max();
                minInput = dataset.DataCollection.Select(D => D.Inputs.Min()).Min();
            }

            if (NormalizeOutputs)
            {
                maxOutput = dataset.DataCollection.Select(D => D.Outputs.Max()).Max();
                minOutput = dataset.DataCollection.Select(D => D.Outputs.Min()).Min();
            }

            max = Max(maxInput, maxOutput);
            min = Min(minInput, minOutput);
        }

        public double[] Normalize(double[] vals)
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
