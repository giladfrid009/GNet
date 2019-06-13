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

            var datasetGenerator = new Datasets.Dynamic.EvenOdd(10);

            var dataset = datasetGenerator.Generate(2000, new Normalizers.None());

            Console.WriteLine(net.Validate(dataset, new Losses.MSE()));

            net.Train(dataset, new Losses.MSE(), new Optimizers.Default(0.4), 1, 2000, 0.0001).Print();

            Console.WriteLine(net.Validate(datasetGenerator.Generate(1000, new Normalizers.None()), new Losses.MSE()));

            Console.ReadKey();
        }
    }
}
