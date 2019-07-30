using System;

namespace GNet
{
    internal class Program
    {
        private static void Main()
        {
            Layer[] layers = new Layer[]
            {
                new Layer(10, new Activations.Identity(), new Initializers.Zero(), new Initializers.Zero()),
                new Dropout(10, new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero(), 0.00),
                new Layer(1, new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Zero())
            };

            Network net = new Network(layers);

            net.Initialize();

            var trainingDataset = new Datasets.Dynamic.EvenOdd(10);
            var validationDataset = new Datasets.Dynamic.EvenOdd(10);
            var validationLoss = new OutTransformers.Losses.BinaryRoundLoss();

            trainingDataset.Generate(2000);
            validationDataset.Generate(1000);

            net.Train(trainingDataset, new Losses.MSE(), new Optimizers.NestrovMomentum(), 30, 1000, 0.01, validationDataset, validationLoss).Print();

            Console.ReadKey();
        }
    }
}
