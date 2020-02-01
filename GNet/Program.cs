using System;
using System.Diagnostics;

namespace GNet
{
    // todo: fix cloning everywhere. make sure that clomning is precise, not just creating same object, butc loning inner objects also
    internal class Program
    {
        private static void Main()
        {
            var net = new Network
            (
                new Layers.Dense(new Shape(10), new Activations.Identity(), new Initializers.Zero(), new Initializers.Zero()),
                new Layers.Dense(new Shape(5, 5), new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero()),
                new Layers.Dense(new Shape(5, 5), new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero()),
                new Layers.Dense(new Shape(5, 5), new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero()),
                new Layers.Dense(new Shape(1), new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Zero())
            );

            var log = new Logger(net);

            net.Initialize();

            var datasetGenerator = new Datasets.Generators.EvenOdd(10);
            Dataset trainingDataset = datasetGenerator.Generate(2000);
            Dataset validationDataset = datasetGenerator.Generate(1000);

            Stopwatch sw = new Stopwatch();

            sw.Start();

            net.Train(trainingDataset, new Losses.MSE(), new Optimizers.NestrovMomentum(), 20, 300, 0.00001, validationDataset, new OutTransformers.Losses.BinaryRoundLoss());

            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);

            Console.ReadKey();
        }
    }
}
