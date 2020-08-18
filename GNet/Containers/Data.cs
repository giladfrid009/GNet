using NCollections;
using System;

namespace GNet
{
    [Serializable]
    public class Data
    {
        public Tensor<double> Inputs { get; }
        public Tensor<double> Targets { get; }
        public Shape InputShape { get; }
        public Shape TargetShape { get; }

        public Data(Tensor<double> inputs, Tensor<double> targets)
        {
            Inputs = inputs;
            Targets = targets;
            InputShape = inputs.Shape;
            TargetShape = targets.Shape;
        }
    }
}