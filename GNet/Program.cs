using System;

namespace GNet
{
    public class Program
    {
        private static void Main()
        {         
            var datasetGenerator = new Datasets.Generators.EvenOdd(new Shape(10), true);
            Dataset tDataset = datasetGenerator.Generate(1000);
            Dataset vDataset = datasetGenerator.Generate(70);

            Network net = new Network
            (
                new Layers.Dense(new Shape(10), new Activations.Identity()),
                new Layers.Dense(new Shape(5), new Activations.Sigmoid()),
                new Layers.Softmax(new Shape(2))
            );

            using (new Logger(net))
            {
                net.Train(tDataset, new Losses.Categorical.CrossEntropy(), new Optimizers.Default(), 1, 10000, 0.01/*, vDataset, new Metrics.Accuracy()*/);
            }

            Console.ReadKey();
        }
    }
}