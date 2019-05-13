using GNet.Extensions;
using GNet.Normalization;
using System;

namespace GNet
{
    public class Data : ICloneable
    {
        public double[] Inputs { get; private set; }
        public double[] Targets { get; private set; }

        public Data() { }

        public Data(double[] inputs, double[] targets, Normalizers inputNormalizer = Normalizers.None)
        {
            Inputs = Normalizer.NormalizeVals(inputs, inputNormalizer);
            Targets = targets.DeepClone();
        }

        public Data(Array inputs, Array targets, Normalizers inputNormalizer = Normalizers.None)
        {
            Inputs = Normalizer.NormalizeVals(inputs.Flatten<double>(), inputNormalizer);
            Targets = targets.Flatten<double>();
        }

        public object Clone()
        {
            return new Data(Inputs, Targets);
        }
    }
}
