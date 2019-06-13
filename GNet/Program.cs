using System;

namespace GNet
{
    internal class Program
    {
        private static void Main()
        {
            Layer[] layers = new Layer[]
            {
                new Layer(10, new Activations.Identity(), new Initializers.One(), new Initializers.Zero()),
                new Layer(10, new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero()),
                new Layer(1, new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Zero())
            };

            Network net = new Network(layers);
            net.Init();

            var trainingDataset = new Datasets.Dynamic.EvenOdd(10);
            trainingDataset.Generate(2000);

            Console.WriteLine(net.Validate(trainingDataset, new Losses.MSE()));

            net.Train(trainingDataset, new Losses.MSE(), new Optimizers.Default(0.4), 1, 2000, 0.0001).Print();

            var validationDataset = new Datasets.Dynamic.EvenOdd(10);
            validationDataset.Generate(1000);

            Console.WriteLine(net.Validate(validationDataset, new Losses.MSE()));

            Console.ReadKey();
        }
    }
}
