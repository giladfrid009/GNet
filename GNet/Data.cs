using GNet.Extensions.Generic;
using System;

namespace GNet
{
    [Serializable]
    public class Data : ICloneable<Data>
    {
        public double[] Inputs { get; }
        public double[] Outputs { get; }

        public Data(double[] inputs, double[] outputs)
        {
            Inputs = inputs.Select(X => X);
            Outputs = outputs.Select(X => X);
        }

        public Data Clone()
        {
            return new Data(Inputs, Outputs);
        }
    }
}
