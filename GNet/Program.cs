using System;

namespace GNet
{
    internal class Program
    {
        private static void Main()
        {
            Dense[] layers = new Dense[]
            {
                new Dense(new Shape(10), new Activations.Identity(), new Initializers.Zero(), new Initializers.Zero()),
                new Dense(new Shape(5,2), new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero()),
                new Dense(new Shape(1), new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Zero())
            };

            Network net = new Network(layers);

            net.Initialize();

            Datasets.Generators.EvenOdd datasetGenerator = new Datasets.Generators.EvenOdd(10);
            Dataset trainingDataset = datasetGenerator.Generate(2000);
            Dataset validationDataset = datasetGenerator.Generate(1000);

            net.Train(trainingDataset, new Losses.MSE(), new Optimizers.NestrovMomentum(), 20, 1000, 0.01, validationDataset, new OutTransformers.Losses.BinaryRoundLoss());

            Console.ReadKey();
        }
    }
}
