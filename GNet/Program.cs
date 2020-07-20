using System;
using GNet.CompGraph;

namespace GNet
{
    public class Program
    {
        private static void Main()
        {
            var datasetGenerator = new Datasets.Generators.EvenOdd(new Shape(2, 5), true);

            Dataset tDataset = datasetGenerator.Generate(5000);
            Dataset vDataset = datasetGenerator.Generate(100);

            Node n = new Node(
                new Layers.Dense(new Shape(2, 5), new Activations.Identity()),
                new Layers.Dense(new Shape(10), new Activations.Sigmoid(), new Initializers.TruncNormal())
                
            );

            Node n2 = new Node(
                new ImmutableArray<Node>(n),     
                new Layers.Concat(new Shape(10)),
                new Layers.Softmax(new Shape(2))             
            );

            var graph = new Graph(n, n2);

            //graph.Predict(new ImmutableShapedArray<double>(new Shape(2, 5), () => 0.5));

            //n2.CalcGrads(new Losses.Regression.MSE(), new ImmutableShapedArray<double>(new Shape(2), 0.5, 0.7));

            //n.Layers[^1].Neurons.ForEach(N => Console.Write(N.Gradient));
            //Console.WriteLine();
            //n2.Layers[0].Neurons.ForEach(N => Console.Write(N.Gradient));


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