using GNet.Extensions;
using System;

namespace GNet
{
    public class Data : ICloneable<Data>
    {
        public double[] Inputs { get; }
        public double[] Targets { get; }
        public INormalizer Normalizer { get; }

        public Data(double[] inputs, double[] targets, INormalizer normalizer)
        {
            Normalizer = normalizer;
            Inputs = Normalizer.Normalize(inputs);
            Targets = targets.Select(T => T);
        }

        public Data(Array inputs, Array targets, INormalizer normalizer)
        {
            Normalizer = normalizer;
            Inputs = Normalizer.Normalize(inputs.Flatten<double>());
            Targets = targets.Flatten<double>();
        }

        public Data Clone() => new Data(Inputs, Targets, Normalizer);

        public static bool VerifyStructure(Data[] dataArray, int inputLength, int outputLength)
        {
            foreach (Data D in dataArray)
            {
                if (D.Inputs.Length != inputLength)
                    return false;

                if (D.Targets.Length != outputLength)
                    return false;
            }

            return true;
        }
    }
}
