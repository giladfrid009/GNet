namespace GNet
{
    public delegate void BatchLogFunc(int batch);
    public delegate void ErrorLogFunc(double error);
    public delegate void EpochErrorLogFunc(int epoch, double error);

    public interface INetwork
    {
        event BatchLogFunc? OnBatch;
        event ErrorLogFunc? OnStart;
        event EpochErrorLogFunc? OnEpoch;
        event EpochErrorLogFunc? OnFinish;

        Shape InputShape { get; }
        Shape OutputShape { get; }

        ImmutableShapedArray<double> Predict(ImmutableShapedArray<double> inputs);

        double Validate(Dataset dataset, IMetric metric);

        void Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int nEpoches, double minError, Dataset valDataset, IMetric metric, bool shuffle = true);

        void Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int nEpoches, double minError, bool shuffle = true);
    }
}
