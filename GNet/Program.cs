using System;

namespace GNet
{
    public class Program
    {
        private static void Main()
        {
            var net2 = new Network
            (
                new Layers.Pooling(new Shape(4,4), new Shape(4,4), new ArrayImmutable<int>(1,1), new ArrayImmutable<int>(0,0), new Layers.Kernels.MaxPool(new Shape(4, 4)))
            );

            net2.Initialize();

            ShapedArrayImmutable<double> x = net2.FeedForward(new ShapedArrayImmutable<double>(new Shape(4, 4), 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));


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