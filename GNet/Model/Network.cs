using System;

namespace GNet
{
    public class Network : ICloneable<Network>
    {
        public delegate void BatchLogger(int batch);

        public delegate void ErrorLogger(double error);

        public delegate void EpochErrorLogger(int epoch, double error);

        public event BatchLogger? OnBatch;

        public event ErrorLogger? OnStart;

        public event EpochErrorLogger? OnEpoch;

        public event EpochErrorLogger? OnFinish;

        public ArrayImmutable<ILayer> Layers { get; }
        public int Length => Layers.Length;

        public Network(params ILayer[] layers)
        {
            Layers = new ArrayImmutable<ILayer>(layers);
            Connect();
        }

        public Network(ArrayImmutable<ILayer> layers)
        {
            Layers = layers;
            Connect();
        }

        private void Connect()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Connect(Layers[i - 1]);
            }
        }

        private void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
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
            //    if (Layers[i].IsTrainable)
            //    {
            //        optimizer.Optimize(Layers[i], epoch);
            //    }
            //});

            for (int i = 1; i < Length; i++)
            {
                if (Layers[i].IsTrainable)
                {
                    optimizer.Optimize(Layers[i], epoch);
                }
            }
        }

        private void Update()
        {
            //Parallel.For(1, Length, i =>
            //{
            //    if (Layers[i].IsTrainable)
            //    {
            //        Layers[i].Update();
            //    }
            //});

            for (int i = 1; i < Length; i++)
            {
                if (Layers[i].IsTrainable)
                {
                    Layers[i].Update();
                }
            }
        }

        public void Initialize()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Initialize();
            }
        }

        public ShapedArrayImmutable<double> FeedForward(ShapedArrayImmutable<double> inputs)
        {
            Layers[0].Input(inputs);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Forward();
            }

            return Layers[Length - 1].Neurons.Select(N => N.ActivatedValue);
        }

        public double Validate(Dataset dataset, ILoss loss)
        {
            return dataset.Sum(D => loss.Compute(D.Outputs, FeedForward(D.Inputs))) / dataset.Length;
        }

        public void Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int nEpoches, double minError, Dataset valDataset, ILoss valLoss, bool shuffle = true)
        {
            if (dataset.InputShape != Layers[0].Shape)
            {
                throw new Exception("Dataset input shape mismatch.");
            }

            if (dataset.OutputShape != Layers[Length - 1].Shape)
            {
                throw new Exception("Dataset output shape mismatch.");
            }

            if (valDataset.InputShape != Layers[0].Shape)
            {
                throw new Exception("ValDataset input shape mismatch.");
            }

            if (valDataset.OutputShape != Layers[Length - 1].Shape)
            {
                throw new Exception("ValDataset output shape mismatch.");
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
                    FeedForward(D.Inputs);
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

        public Network Clone()
        {
            var net = new Network(Layers.Select(L => L.Clone()));

            Layers.ForEach((L, i) => L.Neurons.ForEach((N, j) =>
            {
                N.InSynapses.ForEach((S, k) => S.CopyParams(Layers[i].Neurons[j].InSynapses[k]));
                N.OutSynapses.ForEach((S, k) => S.CopyParams(Layers[i].Neurons[j].OutSynapses[k]));
            }));

            return net;
        }
    }
}