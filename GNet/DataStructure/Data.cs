using System;

namespace GNet
{
    [Serializable]
    public class Data
    {
        public ImmutableShapedArray<double> Inputs { get; }
        public ImmutableShapedArray<double> Targets { get; }
        public Shape InputShape { get; }
        public Shape TargetShape { get; }

        public Data(ImmutableShapedArray<double> inputs, ImmutableShapedArray<double> targets)
        {
            Inputs = inputs;
            Targets = targets;
            InputShape = inputs.Shape;
            TargetShape = targets.Shape;
        }
    }
}