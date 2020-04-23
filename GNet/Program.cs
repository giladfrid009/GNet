using System;

namespace GNet
{
    public class Program
    {
        private static void Main()
        {         
            var datasetGenerator = new Datasets.Generators.EvenOdd(new Shape(10), true);
            Dataset tDataset = datasetGenerator.Generate(100);
            Dataset vDataset = datasetGenerator.Generate(100);

            Network net = new Network
            (
                new Layers.Dense(new Shape(10), new Activations.Identity()),
                new Layers.Dense(new Shape(10), new Activations.Sigmoid()),
                new Layers.Dense(new Shape(10), new Activations.Sigmoid()),
                new Layers.Dense(new Shape(2), new Activations.Softmax())
            );

            using (new Logger(net))
            {
                net.Train(tDataset, new Losses.Regression.MSE(), new Optimizers.Default(), 1, 10000, 0.01/*, vDataset, new OutTransformers.Losses.BinaryMaxLoss()*/);
            }

            net.Forward(new ImmutableShapedArray<double>(new Shape(10), () => 1)).ForEach(X => Console.WriteLine(X));

            net.Forward(new ImmutableShapedArray<double>(new Shape(10), () => 0)).ForEach(X => Console.WriteLine(X));

            Console.ReadKey();
        }
    }
}