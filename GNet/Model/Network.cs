using System;
using GNet.Extensions.IArray;
using GNet.Extensions.IShapedArray;
using GNet.Layers;

namespace GNet
{
    [Serializable]
    public class Network : ICloneable<Network>
    {
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
            //Parallel.For(1, Length, (i) =>
            //{
            //    Layers[i].Connect(Layers[i - 1]);
            //});

            //Parallel.ForEach(Partitioner.Create(1, Length), (range) =>
            //{
            //    for (int i = range.Item1; i < range.Item2; i++)
            //    {
            //        Layers[i].Connect(Layers[i - 1]);
            //    }
            //});

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Connect(Layers[i - 1]);
            }
        }

        public ShapedArrayImmutable<double> FeedForward(ShapedArrayImmutable<double> inputs)
        {
            if (inputs.Shape != Layers[0].Shape)
            {
                throw new ArgumentOutOfRangeException("inputs shape and input layer shape mismatch.");
            }

            Layers[0].SetInputs(inputs);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].FeedForward();
            }

            return Layers[Length - 1].Neurons.Select(N => N.ActivatedValue);
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
            //Parallel.For(1, Length, (i) =>
            //{
            //    optimizer.Optimize(Layers[i], epoch);
            //});

            //Parallel.ForEach(Partitioner.Create(1, Length), (range) =>
            //{
            //    for (int i = range.Item1; i < range.Item2; i++)
            //    {
            //        optimizer.Optimize(Layers[i], epoch);
            //    }
            //});

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

        public Log Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int numEpoches, double minError)
        {
            if (dataset.InputShape != Layers[0].Shape || dataset.OutputShape != Layers[Length - 1].Shape)
            {
                throw new Exception("dataset structure mismatch with network input structure.");
            }

            var trainingLog = new Log();
            double error = Validate(dataset, loss);
            int epoch;

            trainingLog.Add("Training started.", true);
            trainingLog.Add("Initial error: " + error, true);

            for (epoch = 0; epoch < numEpoches; epoch++)
            {
                if (error < minError)
                {
                    trainingLog.Add("Error has reached the destination value.", true);
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
            }

            trainingLog.Add("Epoches completed " + epoch, true);
            trainingLog.Add("Final error: " + error, true);
            trainingLog.Add("Training completed.", true);

            return trainingLog;
        }

        public Log Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int numEpoches, double valMinError, Dataset valDataset, ILoss valLoss)
        {
            if (dataset.InputShape != Layers[0].Shape || dataset.OutputShape != Layers[Length - 1].Shape)
            {
                throw new Exception("dataset structure mismatch with network input structure.");
            }

            if (valDataset.InputShape != Layers[0].Shape || valDataset.OutputShape != Layers[Length - 1].Shape)
            {
                throw new Exception("dataset structure mismatch with network input structure.");
            }

            var trainingLog = new Log();
            double valError = Validate(valDataset, valLoss);
            int epoch;

            trainingLog.Add("Training started.", true);
            trainingLog.Add("Initial error: " + Validate(dataset, loss), true);
            trainingLog.Add("Initial validation error: " + valError, true);

            for (epoch = 0; epoch < numEpoches; epoch++)
            {
                if (valError < valMinError)
                {
                    trainingLog.Add("Validation error has reached the destination value.", true);
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
            }

            trainingLog.Add("Epoches completed " + epoch, true);
            trainingLog.Add("Final error: " + Validate(dataset, loss), true);
            trainingLog.Add("Final validation error: " + valError, true);
            trainingLog.Add("Training completed.", true);

            return trainingLog;
        }

        public Network Clone()
        {
            var newNet = new Network(Layers);

            newNet.Layers.ForEach((L, i) =>
            {
                L.Neurons.ForEach((N, j) =>
                {
                    N.CloneVals(Layers[i].Neurons[j]);
                    N.InSynapses.ForEach((S, k) => S.CloneVals(Layers[i].Neurons[j].InSynapses[k]));
                });
            });

            return newNet;
        }
    }
}
