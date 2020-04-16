using System;

namespace GNet
{
    [Serializable]
    public class Data
    {
        public ImmutableShapedArray<double> Inputs { get; }
        public ImmutableShapedArray<double> Outputs { get; }

        public Data(ImmutableShapedArray<double> inputs, ImmutableShapedArray<double> outputs)
        {
            Inputs = inputs;
            Outputs = outputs;
        }
    }
}