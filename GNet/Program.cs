using System;

namespace GNet
{
    internal class Program
    {
        private static void Main()
        {
            Layer[] layers = new Layer[]
            {
                new Layer(10, new Activations.Identity(), new Initializers.One(), new Initializers.Zero()),
                new Layer(10, new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero()),
                new Layer(1, new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Zero())
            };

            Network net = new Network(layers);
            net.Init();
            net.Clone();

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
