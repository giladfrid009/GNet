using GNet.Utils;
using System;

namespace GNet
{
    public class Program
    {
        private static void Main()
        {
            var datasetGenerator = new Datasets.Generators.EvenOdd(new Shape(2, 5), true);

            Dataset tDataset = datasetGenerator.Generate(5000);
            Dataset vDataset = datasetGenerator.Generate(100);

            var net = new Network
            (
                new Layers.Dense(new Shape(2, 5), new Activations.Identity()),
                new Layers.Dense(new Shape(10), new Activations.Sigmoid(), new Initializers.TruncNormal()),
                new Layers.Softmax(new Shape(2))
            );

            using (new Logger(net))
            {
                net.Train(tDataset, new Losses.Categorical.CrossEntropy(), new Optimizers.AdaGradWindow(), 
                    10, 1000, 0.01, vDataset, new Metrics.Classification.Accuracy());
            }

            Console.ReadKey();
        }
    }
}