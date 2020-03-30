using System;
using GNet.Utils;

namespace GNet
{
    public class Program
    {
        private static void Main()
        {
            var net = new Network
            (
                new Layers.Dense(new Shape(30, 30), new Activations.Identity(), new Initializers.One(), new Initializers.Zero()),
                new Layers.Dense(new Shape(20, 14, 14), new Activations.Tanh(), new Initializers.Normal(), new Initializers.Zero()),
                //new Layers.Convolutional
                //(
                //    new Shape(30, 30),
                //    new Shape(4, 4),
                //    new ArrayImmutable<int>(2, 2),
                //    new ArrayImmutable<int>(0, 0),
                //    20,
                //    new Activations.Tanh(),
                //    new Initializers.Normal(),
                //    new Initializers.Zero()
                //),
                //new Layers.Pooling
                //(
                //    new Shape(20, 14, 14),
                //    new Shape(1, 4, 4),
                //    new ArrayImmutable<int>(1, 2, 2),
                //    new ArrayImmutable<int>(0, 0, 0),
                //    new Layers.Poolers.Max()
                //),
                new Layers.Dense(new Shape(100), new Activations.Tanh(), new Initializers.Normal(), new Initializers.Zero()),
                new Layers.Dense(new Shape(30), new Activations.Tanh(), new Initializers.Normal(), new Initializers.Zero()),
                new Layers.Dense(new Shape(1), new Activations.Tanh(), new Initializers.Normal(), new Initializers.Zero())
            );

            var datasetGenerator = new Datasets.Generators.EvenOdd(new Shape(30, 30));
            Dataset tDataset = datasetGenerator.Generate(600);
            Dataset vDataset = datasetGenerator.Generate(100);

            TimeSpan time = Benchmark.BatchTime(net, N =>
            {
                using (new Logger(net) { LogBatches = true })
                {
                    N.Train(tDataset, new Losses.MSE(), new Optimizers.NestrovMomentum(), 20, 5, 0.001, vDataset, new OutTransformers.Losses.BinaryRoundLoss(), false);
                }

            }, 10);

            Console.WriteLine(time);

            Console.ReadKey();
        }
    }
}