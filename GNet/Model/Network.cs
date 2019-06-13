using GNet.Extensions;
using System;

namespace GNet
{
    public class Network
    {
        public int Length { get; }
        public Layer[] Layers { get; } = new Layer[0];

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
            return dataset.DataCollection.Sum(D => loss.Compute(D.Targets, FeedForward(D.Inputs))) / dataset.Length;
        }

        private void Backprop(ILoss loss, IOptimizer optimizer, double[] targets)
        {
            Layers[Length - 1].Backprop(optimizer, loss, targets);

            for (int i = Length - 2; i > 0; i--)
            {
                Layers[i].Backprop(optimizer);
            }
        }

        public TrainingResult Train(IDataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int numEpoches, double minError, bool shuffle = true)
        {
            var epoch = 0;
            var epochError = 0.0;
            var staringTime = DateTime.Now;

            for (epoch = 0; epoch < numEpoches; epoch++)
            {
                if (shuffle)
                    dataset.DataCollection.Shuffle();

                dataset.DataCollection.ForEach((D, index) =>
                {
                    FeedForward(D.Inputs);

                    Backprop(loss, optimizer, D.Targets);

                    if (index % batchSize == 0)
                    {
                        Layers.ForEach(L => L.Update());
                    }
                });

                epochError = Validate(dataset, loss);

                if (epochError < minError)
                    break;
            }

            return new TrainingResult(epoch, epochError, DateTime.Now - staringTime);
        }
    }
}
