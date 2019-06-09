using GNet.Extensions;
using System;

namespace GNet
{
    public class Data : ICloneable<Data>
    {
        public double[] Inputs { get; }
        public double[] Targets { get; }
        public INormalizer InputNormalizer { get; }
        public INormalizer OutputNormalizer { get; }

        public Data(double[] inputs, double[] targets, INormalizer inputNormalizer = null, INormalizer outputNormalizer = null)
        {
            InputNormalizer = inputNormalizer ?? new Normalizers.None();
            OutputNormalizer = outputNormalizer ?? new Normalizers.None();
            Inputs = InputNormalizer.Normalize(inputs);
            Targets = OutputNormalizer.Normalize(targets);
        }

        public Data(Array inputs, Array targets, INormalizer inputNormalizer = null, INormalizer outputNormalizer = null) : 
            this(inputs.Flatten<double>(), targets.Flatten<double>(), inputNormalizer, outputNormalizer) { }

        public Data Clone() => new Data(Inputs, Targets, InputNormalizer, OutputNormalizer);

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
