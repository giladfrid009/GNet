using GNet.Extensions.Array;
using GNet.Extensions.ShapedArray;
using GNet.Extensions.IArray;
using System;

namespace GNet
{
    [Serializable]
    public class Network : IArray<Dense>, ICloneable<Network>
    {
        public int Length { get; }

        private readonly Dense[] layers;

        public Dense this[int index] => layers[index];

        public Network(Dense[] layers)
        {
            Length = layers.Length;
            this.layers = layers.Select(L => L.Clone());

            Connect();
        }

        public void Initialize()
        {
            for (int i = 1; i < Length; i++)
            {
                layers[i].Initialize();
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
                layers[i].Connect(layers[i - 1]);
            }
        }

        public ShapedArrayImmutable<double> FeedForward(ShapedArrayImmutable<double> inputs)
        {
            if (inputs.Shape.Equals(layers[0].Shape) == false)
            {
                throw new ArgumentOutOfRangeException("inputs shape and input layer shape mismatch.");
            }

            layers[0].SetInputs(inputs);

            for (int i = 1; i < Length; i++)
            {
                layers[i].FeedForward();
            }

            return layers[Length - 1].Neurons.Select(N => N.ActivatedValue);
        }

        public double Validate(Dataset dataset, ILoss loss)
        {
            return dataset.Sum(D => loss.Compute(D.Outputs, FeedForward(D.Inputs))) / dataset.Length;
        }

        private void CalcGrads(ILoss loss, ShapedArrayImmutable<double> targets)
        {
            layers[Length - 1].CalcGrads(loss, targets);

            for (int i = Length - 2; i > 0; i--)
            {
                layers[i].CalcGrads();
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
                optimizer.Optimize(layers[i], epoch);
            }
        }

        private void Update()
        {
            for (int i = 1; i < Length; i++)
            {
                layers[i].Update();
            }
        }

        public Log Train(Dataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int numEpoches, double minError)
        {
            if (dataset.InputShape.Equals(layers[0].Shape) == false || dataset.OutputShape.Equals(layers[Length - 1].Shape) == false)
            {
                throw new Exception("dataset structure mismatch with network input structure.");
            }

            Log trainingLog = new Log();
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
            if (dataset.InputShape.Equals(layers[0].Shape) == false || dataset.OutputShape.Equals(layers[Length - 1].Shape) == false)
            {
                throw new Exception("dataset structure mismatch with network input structure.");
            }

            if (valDataset.InputShape.Equals(layers[0].Shape) == false || valDataset.OutputShape.Equals(layers[Length - 1].Shape) == false)
            {
                throw new Exception("dataset structure mismatch with network input structure.");
            }

            Log trainingLog = new Log();
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
            Network newNet = new Network(layers);

            newNet.layers.ForEach((L, i) =>
            {
                L.Neurons.ForEach((N, j) =>
                {
                    N.CloneVals(layers[i].Neurons[j]);
                    N.InSynapses.ForEach((S, k) => S.CloneVals(layers[i].Neurons[j].InSynapses[k]));
                });
            });

            return newNet;
        }

    }
}
