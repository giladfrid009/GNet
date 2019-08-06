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
                new Layer(10, new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero()),
                new Layer(1, new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Zero())
            };

            Network net = new Network(layers);

            net.Initialize();

            var datasetGenerator = new Datasets.Generators.EvenOdd(10);

            var trainingDataset = datasetGenerator.Generate(2000);
            var validationDataset = datasetGenerator.Generate(1000);
            var validationLoss = new OutTransformers.Losses.BinaryRoundLoss();

            net.Train(trainingDataset, new Losses.MSE(), new Optimizers.NestrovMomentum(), 30, 1000, 0.01, validationDataset, validationLoss);

            Console.ReadKey();
        }
    }
}
