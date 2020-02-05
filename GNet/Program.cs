using System;

namespace GNet
{
    public class Program
    {
        private static void Main()
        {
            var net = new Network
            (
                new Layers.Dense(new Shape(10), new Activations.Identity(), new Initializers.Zero(), new Initializers.Zero()),
                new Layers.Dense(new Shape(5, 2), new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero()),
                new Layers.Dense(new Shape(1), new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Zero())
            );

            var datasetGenerator = new Datasets.Generators.EvenOdd(10);
            Dataset trainingDataset = datasetGenerator.Generate(2000);
            Dataset validationDataset = datasetGenerator.Generate(1000);

            net.Initialize();

            using (new Logger(net) { LogEpoches = true })
            {
                net.Train(trainingDataset, new Losses.MSE(), new Optimizers.NestrovMomentum(), 20, 1000, 0.001, validationDataset, new OutTransformers.Losses.BinaryRoundLoss());
            }

            Console.ReadKey();
        }       
    }
}
