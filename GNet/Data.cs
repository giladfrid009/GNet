using GNet.Extensions.Generic;
using System;

namespace GNet
{
    public class Data : ICloneable<Data>
    {
        public INormalizer InputNormalizer { get; }
        public INormalizer OutputNormalizer { get; }
        public double[] Inputs { get; }
        public double[] Targets { get; }

        public Data(double[] inputs, double[] targets, INormalizer inputNormalizer = null, INormalizer outputNormalizer = null)
        {
            InputNormalizer = inputNormalizer?.Clone();
            OutputNormalizer = outputNormalizer?.Clone();

            Inputs = InputNormalizer?.Normalize(inputs) ?? inputs.Select(X => X);
            Targets = OutputNormalizer?.Normalize(targets) ?? inputs.Select(X => X);            
        }

        public Data(Array inputs, Array targets, INormalizer inputNormalizer = null, INormalizer outputNormalizer = null) : 
            this(inputs.Flatten<double>(), targets.Flatten<double>(), inputNormalizer, outputNormalizer) { }

        public Data Clone() => new Data(Inputs, Targets, InputNormalizer, OutputNormalizer);
    }
}
