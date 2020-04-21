using System;

namespace GNet
{
    public class Program
    {
        private static void Main()
        {
            Func<Network> netCreator = () => new Network
            (
                new Layers.Dense(new Shape(1, 30, 30), new Activations.Identity()),
                new Layers.Convolutional
                (
                    new Shape(1, 30, 30),
                    new Shape(20, 14, 14),
                    new Shape(1, 4, 4),
                    new ImmutableArray<int>(1, 2, 2),
                    new Activations.Tanh()
                ),
                new Layers.Pooling
                (
                    new Shape(20, 14, 14),
                    new Shape(20, 6, 6),
                    new Shape(1, 4, 4),
                    new ImmutableArray<int>(1, 2, 2),
                    new Layers.Poolers.Max()
                ),
                new Layers.Dense(new Shape(100), new Activations.Tanh()),
                new Layers.Dense(new Shape(30), new Activations.Tanh()),
                new Layers.Dense(new Shape(1), new Activations.Tanh())
            );

            var datasetGenerator = new Datasets.Generators.EvenOdd(new Shape(1, 30, 30));
            Dataset tDataset = datasetGenerator.Generate(600);
            Dataset vDataset = datasetGenerator.Generate(100);

            TimeSpan time = Utils.Benchmark.BatchTime(netCreator, N =>
            {
                N.Train(tDataset, new Losses.Regression.MSE(), new Optimizers.NestrovMomentum(), 20, 5, 0.001, vDataset, new OutTransformers.Losses.BinaryRoundLoss(), false);
                Console.WriteLine(".");
            }, 
            10);

            Console.WriteLine(time);

            Console.ReadKey();
        }
    }
}