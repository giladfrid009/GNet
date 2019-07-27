using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using System;

namespace GNet
{
    [Serializable]
    public class Network : ICloneable<Network>
    {
        public Layers.Input InputLayer { get; }
        public Layers.Hidden[] HiddenLayers { get; } = new Layers.Hidden[0];
        public Layers.Output OutputLayer { get; }
        public int Length { get; }

        public Network (Layers.Input inputLayer, Layers.Hidden[] hiddenLayers, Layers.Output outputLayer)
        {
            InputLayer = (Layers.Input)inputLayer.Clone();
            HiddenLayers = hiddenLayers.Select(L => (Layers.Hidden)L.Clone());
            OutputLayer = (Layers.Output)outputLayer.Clone();

            Length = hiddenLayers.Length + 2;
        }        

        public void Init()
        {
            HiddenLayers[0].Init(InputLayer);

            for (int i = 1; i < HiddenLayers.Length; i++)
            {
                HiddenLayers[i].Init(HiddenLayers[i - 1]);
            }

            OutputLayer.Init(HiddenLayers[HiddenLayers.Length - 1]);
        }

        public double[] FeedForward(double[] inputs)
        {
            if (inputs.Length != InputLayer.Length)
                throw new ArgumentOutOfRangeException("Input length and input layer length mismatch.");

            InputLayer.SetInput(inputs);

            for (int i = 0; i < HiddenLayers.Length; i++)
            {
                HiddenLayers[i].FeedForward();
            }

            OutputLayer.FeedForward();

            return OutputLayer.Neurons.Select(N => N.ActivatedValue);
        }

        public double Validate(IDataset dataset, ILoss loss)
        {
            return dataset.DataCollection.Accumulate(0.0, (R, D) => R + loss.Compute(D.Outputs, FeedForward(D.Inputs))) / dataset.DataLength;
        }

        private void FeedBackward(IOptimizer optimizer, ILoss loss, double[] targets, int epoch)
        {
            OutputLayer.FeedBackward(optimizer, loss, targets, epoch);

            for (int i = HiddenLayers.Length - 1; i >= 0; i--)
            {
                HiddenLayers[i].FeedBackward(optimizer, epoch);
            }
        }

        private void Update()
        {
            HiddenLayers.ForEach(L => L.Update());
            OutputLayer.Update();
        }

        public TrainingResult Train(IDataset dataset, ILoss loss, IOptimizer optimizer, int batchSize, int numEpoches, double minError)
        {
            if (dataset.InputLength != InputLayer.Length || dataset.OutputLength != OutputLayer.Length)
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

                    FeedBackward(optimizer, loss, D.Outputs, epoch);

                    if (index % batchSize == 0)
                    {
                        Update();
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
            if (dataset.InputLength != InputLayer.Length || dataset.OutputLength != OutputLayer.Length)
                throw new Exception("Dataset structure mismatch with network structure.");

            if (valDataset.InputLength != InputLayer.Length || valDataset.OutputLength != OutputLayer.Length)
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

                    FeedBackward(optimizer, loss, D.Outputs, epoch);

                    if (index % batchSize == 0)
                    {
                        Update();
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
            Network newNet = new Network(InputLayer, HiddenLayers, OutputLayer);

            newNet.Init();

            newNet.InputLayer.Neurons.ForEach((N, i) =>
            {
                N.CloneVals(InputLayer.Neurons[i]);
            });

            newNet.HiddenLayers.ForEach((L, i) =>
            {
                L.Neurons.ForEach((N, j) =>
                {
                    N.CloneVals(HiddenLayers[i].Neurons[j]);
                    N.InSynapses.ForEach((S, k) => S.CloneVals(HiddenLayers[i].Neurons[j].InSynapses[k]));
                });
            });

            newNet.OutputLayer.Neurons.ForEach((N, i) =>
            {
                N.CloneVals(OutputLayer.Neurons[i]);
                N.InSynapses.ForEach((S, j) => S.CloneVals(OutputLayer.Neurons[i].InSynapses[j]));
            });

            return newNet;
        }
    }
}
