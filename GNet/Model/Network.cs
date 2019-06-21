using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using System;

namespace GNet
{
    public class Network
    {
        public Layer[] Layers { get; } = new Layer[0];
        public int Length { get; }

        public Network(Layer[] layers)
        {
            Length = layers.Length;
            Layers = layers.Select(L => L);
        }

        public void Init()
        {
            for (int i = 1; i < Length; i++)
            {
                Layers[i].Init(Layers[i - 1]);
            }
        }        

        public double[] FeedForward(double[] inputs)
        {
            if (inputs.Length != Layers[0].Length)
                throw new ArgumentOutOfRangeException("input length and input layer length mismatch");

            Layers[0].SetInput(inputs);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].FeedForward();
            }

            return Layers[Length - 1].Neurons.Select(N => N.ActivatedValue);
        }

        public double Validate(IDataset dataset, ILoss loss)
        {
            return dataset.DataCollection.Accumulate(0.0, (R,D) => R + loss.Compute(D.Targets, FeedForward(D.Inputs))) / dataset.Length;
        }

        private void Backprop(IOptimizer optimizer, ILoss loss, double[] targets, int epoch)
        {
            Layers[Length - 1].Backprop(optimizer, loss, targets, epoch);

            for (int i = Length - 2; i > 0; i--)
            {
                Layers[i].Backprop(optimizer, epoch);
            }
        }

        public TrainingResult Train(IDataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int numEpoches, double minError, ILoss validationLoss = null)
        {
            if (dataset.InputLength != Layers[0].Length || dataset.OutputLength != Layers[Length - 1].Length)
                throw new Exception("dataset structure mismatch with net structure.");

            if (validationLoss == null)
                validationLoss = loss;

            var staringTime = DateTime.Now;
            var epoch = 0;
            var error = 0.0;
            var initialError = Validate(dataset, validationLoss);

            for (epoch = 0; epoch < numEpoches; epoch++)
            {
                dataset.DataCollection.Shuffle();

                dataset.DataCollection.ForEach((D, index) =>
                {
                    FeedForward(D.Inputs);

                    Backprop(optimizer, loss, D.Targets, epoch);

                    if (index % batchSize == 0)
                    {
                        Layers.ForEach(L => L.Update());
                    }
                });

                error = Validate(dataset, validationLoss);

                if (error < minError)
                    break;
            }

            return new TrainingResult(epoch, initialError, error, DateTime.Now - staringTime);
        }
    }
}
