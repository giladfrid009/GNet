using System;

namespace GNet
{
    public class Program
    {
        private static void Main()
        {
            var net2 = new Network
            (
                new Layers.Dense(new Shape(4, 4), new Activations.Identity(), new Initializers.One(), new Initializers.Zero()),
                //new Layers.Pooling(new Shape(4, 4), new ArrayImmutable<int>(1, 1), new ArrayImmutable<int>(1, 1), new Layers.Poolers.Max()),
                new Layers.Convolutional(3, new Shape(2,2), new ArrayImmutable<int>(2,2), new ArrayImmutable<int>(0,0), new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Normal())
            );

            net2.Initialize();

            var trainingData = new Dataset(new Data(new ShapedArrayImmutable<double>(new Shape(4, 4), 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15),
                new ShapedArrayImmutable<double>(new Shape(2, 2, 2), 0, 0, 0, 0, 0, 0, 0, 0)));

            ShapedArrayImmutable<double> x = net2.FeedForward(trainingData[0].Inputs);


            //using (new Logger(net2) { LogEpoches = true })
            //{
            //    net2.Train(trainingData, new Losses.MSE(), new Optimizers.Default(), 1, 1000, 0.001);
            //}

            var net = new Network
            (
                new Layers.Dense(new Shape(10), new Activations.Identity(), new Initializers.Zero(), new Initializers.Zero()),
                new Layers.Dense(new Shape(5, 2), new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero()),
                new Layers.Dense(new Shape(1), new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Zero())
            );

            var datasetGenerator = new Datasets.Generators.EvenOdd(new Shape(10));
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