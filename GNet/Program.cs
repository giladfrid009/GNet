using GNet.CompGraph;
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

            var n1 = new Node(
                new Layers.Dense(new Shape(2, 5), new Activations.Identity()),
                new Layers.Dense(new Shape(10), new Activations.Sigmoid(), new Initializers.TruncNormal())
            );

            var n2 = new Node
            (
                new ImmutableArray<Node>(n1),
                new Layers.Concat(new Shape(10)),
                new Layers.Softmax(new Shape(2))
            );

            var graph = new Graph(n1, n2);

            //var net = new Sequential
            //(
            //    new Layers.Dense(new Shape(2, 5), new Activations.Identity()),
            //    new Layers.Dense(new Shape(10), new Activations.Sigmoid(), new Initializers.TruncNormal()),
            //    new Layers.Softmax(new Shape(2))
            //);

            using (new Logger(graph))
            {
                graph.Train(tDataset, new Losses.Regression.MSE(), new Optimizers.AdaGradWindow(),
                    1, 1000, 0.01, vDataset, new Metrics.Classification.Accuracy());
            }

            Console.ReadKey();
        }
    }
}