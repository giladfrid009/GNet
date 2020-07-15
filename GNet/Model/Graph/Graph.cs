using System;

namespace GNet.CompGraph
{
    [Serializable]
    public class Graph : INetwork
    {
        public event BatchLogFunc? OnBatch;
        public event ErrorLogFunc? OnStart;
        public event EpochErrorLogFunc? OnEpoch;
        public event EpochErrorLogFunc? OnFinish;

        public Node InputNode { get; }
        public Node OutputNode { get; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }

        public Graph(Node inNode, Node outNode)
        {
            InputNode = inNode;
            OutputNode = outNode;
            InputShape = inNode.InputShape;
            OutputShape = outNode.OutputShape;
        }

        public ImmutableShapedArray<double> Predict(ImmutableShapedArray<double> inputs)
        {
            InputNode.Forward(inputs, false);

            return OutputNode.Layers[OutputNode.Length - 1].Neurons.Select(N => N.OutVal).ToShape(OutputShape);
        }

        public double Validate(Dataset dataset, IMetric metric)
        {
            return dataset.Avarage(D => metric.Evaluate(D.Targets, Predict(D.Inputs)));
        }

        public void Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int nEpoches, double minError, Dataset valDataset, IMetric metric, bool shuffle = true)
        {
            if (dataset.InputShape != InputShape)
            {
                throw new ShapeMismatchException($"{nameof(dataset)} {nameof(dataset.InputShape)}");
            }

            if (dataset.TargetShape != OutputShape)
            {
                throw new ShapeMismatchException($"{nameof(dataset)} {nameof(dataset.TargetShape)}");
            }

            if (valDataset.InputShape != InputShape)
            {
                throw new ShapeMismatchException($"{nameof(valDataset)} {nameof(dataset.InputShape)}");
            }

            if (valDataset.TargetShape != OutputShape)
            {
                throw new ShapeMismatchException($"{nameof(valDataset)} {nameof(dataset.TargetShape)}");
            }

            InputNode.ClearCache();

            double valError = Validate(valDataset, metric);
            int epoch;

            OnStart?.Invoke(valError);

            for (epoch = 0; epoch < nEpoches; epoch++)
            {
                if (valError <= minError)
                {
                    break;
                }

                if (shuffle)
                {
                    dataset.Shuffle();
                }

                dataset.ForEach((D, index) =>
                {
                    InputNode.Forward(D.Inputs, true);
                    OutputNode.CalcGrads(loss, D.Targets);
                    InputNode.Optimize(optimizer, epoch);

                    if (index % batchSize == 0)
                    {
                        InputNode.Update();

                        OnBatch?.Invoke(index / batchSize);
                    }
                });

                valError = Validate(valDataset, metric);

                OnEpoch?.Invoke(epoch, valError);
            }

            OnFinish?.Invoke(epoch, valError);
        }

        public void Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int nEpoches, double minError, bool shuffle = true)
        {
            Train(dataset, loss, optimizer, batchSize, nEpoches, minError, dataset, loss, shuffle);
        }
    }
}
