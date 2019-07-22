using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using System;

namespace GNet
{
    public class Network : ICloneable<Network>
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
                throw new ArgumentOutOfRangeException("Input length and input layer length mismatch.");

            Layers[0].SetInput(inputs);

            for (int i = 1; i < Length; i++)
            {
                Layers[i].FeedForward();
            }

            return Layers[Length - 1].Neurons.Select(N => N.ActivatedValue);
        }

        public double Validate(IDataset dataset, ILoss loss)
        {
            return dataset.DataCollection.Accumulate(0.0, (R, D) => R + loss.Compute(D.Outputs, FeedForward(D.Inputs))) / dataset.DataLength;
        }

        private void Backprop(IOptimizer optimizer, ILoss loss, double[] targets, int epoch)
        {
            Layers[Length - 1].Backprop(optimizer, loss, targets, epoch);

            for (int i = Length - 2; i > 0; i--)
            {
                Layers[i].Backprop(optimizer, epoch);
            }
        }

        public TrainingResult Train(IDataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int numEpoches, double minError)
        {
            if (dataset.InputLength != Layers[0].Length || dataset.OutputLength != Layers[Length - 1].Length)
                throw new Exception("Dataset structure mismatch with network structure.");

            var staringTime = DateTime.Now;
            var error = 0.0;
            var initialError = Validate(dataset, loss);

            int epoch;
            for (epoch = 0; epoch < numEpoches; epoch++)
            {
                dataset.DataCollection.Shuffle();

                dataset.DataCollection.ForEach((D, index) =>
                {
                    FeedForward(D.Inputs);

                    Backprop(optimizer, loss, D.Outputs, epoch);

                    if (index % batchSize == 0)
                    {
                        Layers.ForEach(L => L.Update());
                    }
                });

                error = Validate(dataset, loss);

                if (error < minError)
                    break;
            }

            return new TrainingResult(DateTime.Now - staringTime, epoch, initialError, error, double.NaN);
        }

        public TrainingResult Train(IDataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int numEpoches, double valMinError, IDataset valDataset, ILoss valLoss)
        {
            if (dataset.InputLength != Layers[0].Length || dataset.OutputLength != Layers[Length - 1].Length)
                throw new Exception("Dataset structure mismatch with network structure.");

            if (valDataset.InputLength != Layers[0].Length || valDataset.OutputLength != Layers[Length - 1].Length)
                throw new Exception("ValDataset structure mismatch with network structure.");

            var staringTime = DateTime.Now;
            var valError = 0.0;
            var initialError = Validate(dataset, loss);

            int epoch;
            for (epoch = 0; epoch < numEpoches; epoch++)
            {
                dataset.DataCollection.Shuffle();

                dataset.DataCollection.ForEach((D, index) =>
                {
                    FeedForward(D.Inputs);

                    Backprop(optimizer, loss, D.Outputs, epoch);

                    if (index % batchSize == 0)
                    {
                        Layers.ForEach(L => L.Update());
                    }
                });

                valError = Validate(valDataset, valLoss);

                if (valError < valMinError)
                    break;
            }

            var finalError = Validate(dataset, loss);

            return new TrainingResult(DateTime.Now - staringTime, epoch, initialError, finalError, valError);
        }        

        public Network Clone()
        {
            Network Net = new Network(Layers.Select(L => Layer.Like(L)));

            for (int i = 1; i < Length; i++)
            {
                Net.Layers[i].Connect(Net.Layers[i - 1]);

                Net.Layers[i].Neurons.ForEach((N, j) =>
                {
                    N.InSynapses = N.InSynapses.Select((S, k) => Synapse.Like(S.InNeuron, S.OutNeuron, Layers[i].Neurons[j].InSynapses[k]));
                });
            }

            Net.Layers[Length - 1].Neurons.ForEach((N, j) =>
            {
                N.OutSynapses = N.OutSynapses.Select((S, k) => Synapse.Like(S.InNeuron, S.OutNeuron, Layers[Length - 1].Neurons[j].OutSynapses[k]));
            });

            return Net;
        }        
    }
}
