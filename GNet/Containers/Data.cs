using System;

namespace GNet
{
    [Serializable]
    public class Data
    {
        public ShapedArray<double> Inputs { get; }
        public ShapedArray<double> Targets { get; }
        public Shape InputShape { get; }
        public Shape TargetShape { get; }

        public Data(ShapedArray<double> inputs, ShapedArray<double> targets)
        {
            Inputs = inputs;
            Targets = targets;
            InputShape = inputs.Shape;
            TargetShape = targets.Shape;
        }
    }
}