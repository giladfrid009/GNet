using System;

namespace GNet
{
    [Serializable]
    public class Data
    {
        //todo: go over the targets and check where labels are named outputs.
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