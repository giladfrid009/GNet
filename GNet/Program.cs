using System;

namespace GNet
{
    public class Program
    {
        private static void Main()
        {
            var datasetGenerator = new Datasets.Generators.EvenOdd(new Shape(6), true);
            Dataset tDataset = datasetGenerator.Generate(100);
            Dataset vDataset = datasetGenerator.Generate(50);

            Network net = new Network
            (
                new Layers.Dense(new Shape(6), new Activations.Identity()),
                new Layers.Dense(new Shape(5), new Activations.Sigmoid()),
                new Layers.Softmax(new Shape(2))
            );

            //todo: some optimizers first lead to even worse results. why?
            using (new Logger(net))
            {
                net.Train(tDataset, new Losses.Categorical.CrossEntropy(), new Optimizers.AdaDelta(), 20, 100000, 0.01, vDataset, new Metrics.Classification.Accuracy());
            }

            Console.ReadKey();
        }
    }
}