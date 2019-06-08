using GNet.Extensions;
using System;

namespace GNet
{
    public class Data : ICloneable<Data>
    {
        public readonly double[] Inputs;
        public readonly double[] Targets;
        public readonly INormalizer Normalizer;

        public Data(double[] inputs, double[] targets, INormalizer normalizer = null)
        {
            Normalizer = normalizer ?? new Normalizers.None();

            Inputs = Normalizer.Normalize(inputs);
            Targets = targets.Select(T => T);
        }

        public Data(Array inputs, Array targets, INormalizer normalizer = null)
        {
            Normalizer = normalizer ?? new Normalizers.None();

            Inputs = Normalizer.Normalize(inputs.Flatten<double>());
            Targets = targets.Flatten<double>();
        }

        public Data Clone() => new Data(Inputs, Targets, Normalizer);
    }
}
