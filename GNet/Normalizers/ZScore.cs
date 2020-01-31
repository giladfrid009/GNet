using static System.Math;

namespace GNet.Normalizers
{
    public class ZScore : INormalizer
    {
        public bool NormalizeInputs { get; set; }
        public bool NormalizeOutputs { get; set; }

        private double mean;
        private double sd;

        public void ExtractParams(Dataset dataset)
        {
            double meanInput = 0;
            double meanOutput = 0;
            double varianceInput = 0;
            double varianceOutput = 0;
            double numVals = 0;

            if (NormalizeInputs)
            {
                meanInput = dataset.DataCollection.Sum(D => D.Inputs.Avarage()) / dataset.Length;
            }

            if (NormalizeOutputs)
            {
                meanOutput = dataset.DataCollection.Sum(D => D.Outputs.Avarage()) / dataset.Length;
            }

            mean = (meanInput + meanOutput) / 2;

            if (NormalizeInputs)
            {
                varianceInput = dataset.DataCollection.Sum(D => D.Inputs.Sum(X => (X - mean) * (X - mean)));
                numVals += dataset.InputShape.Volume;
            }

            if (NormalizeOutputs)
            {
                varianceOutput = dataset.DataCollection.Sum(D => D.Outputs.Sum(X => (X - mean) * (X - mean)));
                numVals += dataset.OutputShape.Volume;
            }

            numVals *= dataset.Length;

            sd = Sqrt((varianceInput + varianceOutput) / numVals);
        }

        public ShapedArrayImmutable<double> Normalize(ShapedArrayImmutable<double> vals)
        {
            return vals.Select(X => (X - mean) / sd);
        }

        public INormalizer Clone()
        {
            return new ZScore()
            {
                NormalizeInputs = NormalizeInputs,
                NormalizeOutputs = NormalizeOutputs,
                mean = mean,
                sd = sd
            };
        }
    }
}
