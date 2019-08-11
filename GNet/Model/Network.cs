using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using System;

namespace GNet
{
    [Serializable]
    public class Network : ICloneable<Network>
    {
        public Dense[] Layers { get; } = new Dense[0];
        public int Length { get; }

        public Network(Dense[] layers)
        {
            Length = layers.Length;
            Layers = layers.Select(L => L.Clone());

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

            for (int i = 1; i < Length; i++)
            {
                Layers[i].Connect(Layers[i - 1]);
            }
        }

        public double[] FeedForward(double[] inputs)
        {
            if (inputs.Length != Layers[0].Length)
            {
                throw new ArgumentOutOfRangeException("Input length and input layer length mismatch.");
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
            return dataset.DataCollection.Accumulate(0.0, (R, D) => R + loss.Compute(D.Outputs, FeedForward(D.Inputs))) / dataset.DataLength;
        }

        private void CalcGrads(ILoss loss, double[] targets)
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
            if (dataset.InputLength != Layers[0].Length || dataset.OutputLength != Layers[Length - 1].Length)
            {
                throw new Exception("Dataset structure mismatch with network structure.");
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

                dataset.DataCollection.Shuffle();

                dataset.DataCollection.ForEach((D, index) =>
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
            if (dataset.InputLength != Layers[0].Length || dataset.OutputLength != Layers[Length - 1].Length)
            {
                throw new Exception("Dataset structure mismatch with network structure.");
            }

            if (valDataset.InputLength != Layers[0].Length || valDataset.OutputLength != Layers[Length - 1].Length)
            {
                throw new Exception("ValDataset structure mismatch with network structure.");
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

                dataset.DataCollection.Shuffle();

                dataset.DataCollection.ForEach((D, index) =>
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
            Network newNet = new Network(Layers);

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
