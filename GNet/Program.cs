using System;

namespace GNet
{
    public class Program
    {
        private static void Main()
        {         
            var datasetGenerator = new Datasets.Generators.EvenOdd(new Shape(4), true);
            Dataset tDataset = datasetGenerator.Generate(30);
            Dataset vDataset = datasetGenerator.Generate(30);

            Network net = new Network
            (
                new Layers.Dense(new Shape(4), new Activations.Identity()),
                new Layers.Dense(new Shape(4), new Activations.Sigmoid()),
                new Layers.Dense(new Shape(2), new Activations.Softmax())
            );

            using (new Logger(net))
            {
                net.Train(tDataset, new Losses.Categorical.CrossEntropy(), new Optimizers.Default(), 1, 10000, 0.01, vDataset, new Metrics.ArgMaxAccuracy());
            }

            net.Forward(new ImmutableShapedArray<double>(new Shape(4), () => 1)).ForEach(X => Console.WriteLine(X));

            net.Forward(new ImmutableShapedArray<double>(new Shape(4), () => 0)).ForEach(X => Console.WriteLine(X));

            Console.ReadKey();
        }
    }
}