using System;

namespace GNet
{
    [Serializable]
    public class Data
    {
        public ShapedArrayImmutable<double> Inputs { get; }
        public ShapedArrayImmutable<double> Outputs { get; }

        public Data(ShapedArrayImmutable<double> inputs, ShapedArrayImmutable<double> outputs)
        {
            Inputs = inputs;
            Outputs = outputs;
        }
    }
}