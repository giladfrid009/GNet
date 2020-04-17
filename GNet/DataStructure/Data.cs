using System;

namespace GNet
{
    [Serializable]
    public class Data
    {
        public ImmutableShapedArray<double> Inputs { get; }
        public ImmutableShapedArray<double> Outputs { get; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }

        public Data(ImmutableShapedArray<double> inputs, ImmutableShapedArray<double> outputs)
        {
            Inputs = inputs;
            Outputs = outputs;
            InputShape = inputs.Shape;
            OutputShape = outputs.Shape;
        }
    }
}