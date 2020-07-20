using System;

namespace GNet
{
    public delegate void BatchLogFunc(int batch);
    public delegate void ErrorLogFunc(double error);
    public delegate void EpochErrorLogFunc(int epoch, double error);

    [Serializable]
    public abstract class Network
    {
        public event BatchLogFunc? OnBatch;
        public event ErrorLogFunc? OnStart;
        public event EpochErrorLogFunc? OnEpoch;
        public event EpochErrorLogFunc? OnFinish;

        public Shape InputShape { get; }
        public Shape OutputShape { get; }

        protected Network(Shape inShape, Shape outShape)
        {
            InputShape = inShape;
            OutputShape = outShape;
        }

        protected abstract void Forward(ImmutableShapedArray<double> inputs, bool isTraining);

        protected abstract void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets);

        protected abstract void Optimize(IOptimizer optimizer);

        protected abstract void Update();

        protected abstract void ClearCache();

        protected abstract ImmutableShapedArray<double> GetOutput();

        public ImmutableShapedArray<double> Predict(ImmutableShapedArray<double> inputs)
        {
            Forward(inputs, false);
            return GetOutput();
        }

        public double Validate(Dataset dataset, IMetric metric)
        {
            return dataset.Average(D => metric.Evaluate(D.Targets, Predict(D.Inputs)));
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

            ClearCache();

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
                    Forward(D.Inputs, true);
                    CalcGrads(loss, D.Targets);
                    optimizer.UpdateEpoch(epoch);
                    Optimize(optimizer);

                    if (index % batchSize == 0)
                    {
                        Update();

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
