using System;

namespace GNet.CompGraph
{
    [Serializable]
    public class Graph : Network
    {
        public Node InputNode { get; }
        public Node OutputNode { get; }

        public Graph(Node inNode, Node outNode) : base(inNode.InputShape, outNode.OutputShape)
        {
            InputNode = inNode;
            OutputNode = outNode;

            inNode.Connect();
        }

        protected override void Forward(in ImmutableShapedArray<double> inputs, bool isTraining)
        {
            InputNode.Forward(inputs, isTraining);
        }

        protected override void CalcGrads(ILoss loss, in ImmutableShapedArray<double> targets)
        {
            OutputNode.CalcGrads(loss, targets);
        }

        protected override void Optimize(IOptimizer optimizer)
        {
            InputNode.Optimize(optimizer);
        }

        protected override void Update()
        {
            InputNode.Update();
        }

        protected override void ClearCache()
        {
            InputNode.ClearCache();
        }

        protected override ImmutableShapedArray<double> GetOutput()
        {
            return OutputNode.Layers[OutputNode.Length - 1].Neurons.Select(N => N.OutVal).ToShape(OutputShape);
        }
    }
}