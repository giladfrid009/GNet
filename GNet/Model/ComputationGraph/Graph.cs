using System;

namespace GNet.ComputaionGraph
{
    [Serializable]
    public class Graph : Network
    {        
        public Pipeline InputPipe { get; }
        public Pipeline OutputPipe { get; }

        public Graph(Pipeline inPipe, Pipeline outPipe) : base(inPipe.InputShape, outPipe.OutputShape)
        {
            InputPipe = inPipe;
            OutputPipe = outPipe;

            inPipe.ResetProcessed();
            inPipe.InitOutNodes();
        }

        protected override void Forward(ImmutableShapedArray<double> inputs, bool isTraining)
        {
            InputPipe.ResetProcessed();
            InputPipe.Forward(inputs, isTraining);
        }

        protected override void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            InputPipe.ResetProcessed();
            OutputPipe.CalcGrads(loss, targets);
        }

        protected override void Optimize(IOptimizer optimizer)
        {
            InputPipe.ResetProcessed();
            InputPipe.Optimize(optimizer);
        }

        protected override void Update()
        {
            InputPipe.ResetProcessed();
            InputPipe.Update();
        }

        protected override void ClearCache()
        {
            InputPipe.ResetProcessed();
            InputPipe.ClearCache();
        }

        protected override ImmutableShapedArray<double> GetOutput()
        {
            return OutputPipe.Layers[OutputPipe.Length - 1].Neurons.Select(N => N.OutVal).ToShape(OutputShape);
        }
    }
}
