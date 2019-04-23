using GNet.Extensions;
using System;

namespace GNet
{
    public class Data
    {
        public double[] Inputs { get; private set; }
        public double[] Targets { get; private set; }

        public Data() { }

        public Data(double[] inputs, double[] targets)
        {
            Inputs = inputs.DeepClone();
            Targets = targets.DeepClone();
        }

        public Data(Array inputs, Array targets)
        {
            Inputs = inputs.Flatten<double>();
            Targets = targets.Flatten<double>();
        }
    }
}
