using System;

namespace GNet
{
    [Serializable]
    public class Network
    {
        public delegate void BatchLogger(int batch);
        public delegate void ErrorLogger(double error);
        public delegate void EpochErrorLogger(int epoch, double error);

        public event BatchLogger? OnBatch;
        public event ErrorLogger? OnStart;
        public event EpochErrorLogger? OnEpoch;
        public event EpochErrorLogger? OnFinish;

        public ImmutableArray<ILayer> Layers { get; }
        public Shape InputShape { get; }
        public Shape OutputShape { get; }
        public int Length { get; }

        public Network(ImmutableArray<ILayer> layers)
        {
            Layers = layers;
            Length = layers.Length;
            InputShape = layers[0].Shape;
            OutputShape = layers[layers.Length - 1].Shape;

            Connect();
            Initialize();
        }

        public Network(params ILayer[] layers) : this(new ImmutableArray<ILayer>(layers))
        {
        }
       
        private void Connect()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Connect(Layers[i - 1]);
            }
        }

        private void Initialize()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Initialize();
            }
        }

        public ImmutableShapedArray<double> Forward(ImmutableShapedArray<double> inputs)
        {
            Layers[0].Input(inputs);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Forward();
            }

            return Layers[Length - 1].Neurons.Select(N => N.OutVal).ToShape(OutputShape);
        }

        public double Validate(Dataset dataset, ILoss loss)
        {
            return dataset.Sum(D => loss.Compute(D.Outputs, Forward(D.Inputs))) / dataset.Length;
        }

        public void Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int nEpoches, double minError, Dataset valDataset, ILoss valLoss, bool shuffle = true)
        {
            if (dataset.InputShape != Layers[0].Shape)
            {
                throw new ShapeMismatchException($"{nameof(dataset)} {nameof(dataset.InputShape)}");
            }

            if (dataset.OutputShape != Layers[Length - 1].Shape)
            {
                throw new ShapeMismatchException($"{nameof(dataset)} {nameof(dataset.OutputShape)}");
            }

            if (valDataset.InputShape != Layers[0].Shape)
            {
                throw new ShapeMismatchException($"{nameof(valDataset)} {nameof(dataset.InputShape)}");
            }

            if (valDataset.OutputShape != Layers[Length - 1].Shape)
            {
                throw new ShapeMismatchException($"{nameof(valDataset)} {nameof(dataset.OutputShape)}");
            }

            double valError = Validate(valDataset, valLoss);
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
                    Forward(D.Inputs);
                    CalcGrads(loss, D.Outputs);
                    Optimize(optimizer, epoch);

                    if (index % batchSize == 0)
                    {
                        Update();

                        OnBatch?.Invoke(index / batchSize);
                    }
                });

                valError = Validate(valDataset, valLoss);

                OnEpoch?.Invoke(epoch, valError);
            }

            OnFinish?.Invoke(epoch, valError);
        }

        public void Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int nEpoches, double minError, bool shuffle = true)
        {
            Train(dataset, loss, optimizer, batchSize, nEpoches, minError, dataset, loss, shuffle);
        }

        private void CalcGrads(ILoss loss, ImmutableShapedArray<double> targets)
        {
            Layers[Length - 1].CalcGrads(loss, targets);

            for (int i = Length - 2; i > 0; i--)
            {
                Layers[i].CalcGrads();
            }
        }

        private void Optimize(IOptimizer optimizer, int epoch)
        {
            //Parallel.For(1, Length, i =>
            //{
            //    Layers[i].Optimize(optimizer);
            //});

            optimizer.UpdateParams(epoch);

            for (int i = 1; i < Length; i++)
            {       
                Layers[i].Optimize(optimizer);
            }
        }

        private void Update()
        {
            //Parallel.For(1, Length, i =>
            //{
            //    Layers[i].Update();
            //});

            for (int i = 1; i < Length; i++)
            {         
                Layers[i].Update();             
            }
        }
    }
}