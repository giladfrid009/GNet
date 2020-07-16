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

            inNode.InitOutNodes();
        }     

        protected override void Forward(ImmutableShapedArray<double> inputs, bool isTraining)
        {
            InputNode.Forward(inputs, isTraining);
        }

        protected override void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            OutputNode.CalcGrads(loss, targets);
        }

        protected override void Optimize(IOptimizer optimizer, int epoch)
        {
            optimizer.UpdateParams(epoch);
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

        public override ImmutableShapedArray<double> Predict(ImmutableShapedArray<double> inputs)
        {
            InputNode.Forward(inputs, false);

            return OutputNode.Layers[OutputNode.Length - 1].Neurons.Select(N => N.OutVal).ToShape(OutputShape);
        }
    }
}
