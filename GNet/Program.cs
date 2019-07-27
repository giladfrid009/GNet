using System;

namespace GNet
{
    internal class Program
    {
        private static void Main()
        {
            var inputLayer = new Layers.Input(10);
            var hiddenLayers = new Layers.Hidden[] 
            {
                new Layers.Hidden(10, new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero())
            };
            var outputLayer = new Layers.Output(1, new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Zero());

            Network net = new Network(inputLayer, hiddenLayers, outputLayer);

            net.Init();

            var trainingDataset = new Datasets.Dynamic.EvenOdd(10);
            var validationDataset = new Datasets.Dynamic.EvenOdd(10);
            var validationLoss = new OutTransformers.Losses.BinaryRoundLoss();

            trainingDataset.Initialize(2000);
            validationDataset.Initialize(1000);

            net.Train(trainingDataset, new Losses.MSE(), new Optimizers.NestrovMomentum(), 30, 1000, 0.01, validationDataset, validationLoss).Print();

            Console.ReadKey();
        }
    }
}
