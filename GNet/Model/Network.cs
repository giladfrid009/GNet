using System;
using GNet.Layers;

namespace GNet
{
    public class Network : ICloneable<Network>
    {
        public delegate void ErrorFunc(double error);
        public delegate void EpochErrorFunc(int epoch, double error);

        public event ErrorFunc? OnStart;
        public event EpochErrorFunc? OnEpoch;
        public event EpochErrorFunc? OnFinish;

        public ArrayImmutable<Dense> Layers { get; }
        public int Length => Layers.Length;

        public Network(params Dense[] layers)
        {
            Layers = new ArrayImmutable<Dense>(layers);
            Connect();
        }

        public Network(ArrayImmutable<Dense> layers)
        {
            Layers = layers;
            Connect();
        }

        public void Initialize()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Initialize();
            }
        }

        private void Connect()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Connect(Layers[i - 1]);
            }
        }
        
        public ShapedArrayImmutable<double> FeedForward(ShapedArrayImmutable<double> inputs)
        {
            if (inputs.Shape != Layers[0].InputShape)
            {
                throw new ArgumentOutOfRangeException("inputs shape and input layer shape mismatch.");
            }

            Layers[0].Input(inputs);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Forward();
            }

            return Layers[Length - 1].OutNeurons.Select(N => N.ActivatedValue);
        }

        public double Validate(Dataset dataset, ILoss loss)
        {
            return dataset.Sum(D => loss.Compute(D.Outputs, FeedForward(D.Inputs))) / dataset.Length;
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
            for (int i = 1; i < Length; i++)
            {
                optimizer.Optimize(Layers[i], epoch);
            }
        }

        private void Update()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Update();
            }
        }

        public void Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int numEpoches, double minError)
        {
            if (dataset.InputShape != Layers[0].InputShape || dataset.OutputShape != Layers[Length - 1].InputShape)
            {
                throw new Exception("dataset structure mismatch with network input structure.");
            }

            double error = Validate(dataset, loss);
            int epoch;

            OnStart?.Invoke(error);

            for (epoch = 0; epoch < numEpoches; epoch++)
            {
                if (error <= minError)
                {
                    break;
                }

                dataset.Shuffle();

                dataset.ForEach((D, index) =>
                {
                    FeedForward(D.Inputs);
                    CalcGrads(loss, D.Outputs);
                    Optimize(optimizer, epoch);

                    if (index % batchSize == 0)
                    {
                        Update();
                    }
                });

                error = Validate(dataset, loss);

                OnEpoch?.Invoke(epoch, error);
            }

            OnFinish?.Invoke(epoch, error);
        }

        public void Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int numEpoches, double valMinError, Dataset valDataset, ILoss valLoss)
        {
            if (dataset.InputShape != Layers[0].InputShape || dataset.OutputShape != Layers[Length - 1].InputShape)
            {
                throw new Exception("dataset structure mismatch with network input structure.");
            }

            if (valDataset.InputShape != Layers[0].InputShape || valDataset.OutputShape != Layers[Length - 1].InputShape)
            {
                throw new Exception("dataset structure mismatch with network input structure.");
            }

            double valError = Validate(valDataset, valLoss);
            int epoch;

            OnStart?.Invoke(valError);

            for (epoch = 0; epoch < numEpoches; epoch++)
            {
                if (valError <= valMinError)
                {
                    break;
                }

                dataset.Shuffle();

                dataset.ForEach((D, index) =>
                {
                    FeedForward(D.Inputs);
                    CalcGrads(loss, D.Outputs);
                    Optimize(optimizer, epoch);

                    if (index % batchSize == 0)
                    {
                        Update();
                    }
                });

                valError = Validate(valDataset, valLoss);

                OnEpoch?.Invoke(epoch, valError);
            }

            OnFinish?.Invoke(epoch, valError);
        }

        public Network Clone()
        {
            // todo: implement
            throw new NotImplementedException();
        }
    }
}
