using System;

namespace GNet.CompGraph
{
    [Serializable]
    public class Graph : BaseNetwork
    {        
        public Node InputNode { get; }
        public Node OutputNode { get; }

        public Graph(Node inNode, Node outNode) : base(inNode.InputShape, outNode.OutputShape)
        {
            InputNode = inNode;
            OutputNode = outNode;
        }     

        protected sealed override void Forward(ImmutableShapedArray<double> inputs, bool isTraining)
        {
            InputNode.Forward(inputs, isTraining);
        }

        protected sealed override void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            OutputNode.CalcGrads(loss, targets);
        }

        protected sealed override void Optimize(IOptimizer optimizer, int epoch)
        {
            optimizer.UpdateParams(epoch);
            InputNode.Optimize(optimizer);
        }

        protected sealed override void Update()
        {
            InputNode.Update();
        }

        protected sealed override void ClearCache()
        {
            InputNode.ClearCache();
        }

        public sealed override ImmutableShapedArray<double> Predict(ImmutableShapedArray<double> inputs)
        {
            InputNode.Forward(inputs, false);

            return OutputNode.Layers[OutputNode.Length - 1].Neurons.Select(N => N.OutVal).ToShape(OutputShape);
        }
    }
}
