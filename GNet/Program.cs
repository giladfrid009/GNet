using System;

namespace GNet
{
    internal class Program
    {
        private static void Main()
        {
            Layer[] layers = new Layer[]
            {
                new Layer(1, new Activations.Identity(), new Initializers.One(), new Initializers.Zero()),
                new Layer(6, new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Uniform()),
                new Layer(6, new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Uniform()),
                new Layer(1, new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Uniform())
            };

            Network net = new Network(layers);
            net.Init();

            var dataset = Datasets.MathOperations.GenerateDataset(Datasets.MathOperations.Ops1.Sin, 100, 1000, new Normalizers.Division(100));

            Console.WriteLine(net.Validate(dataset, new Losses.MSE()));

            net.Train(dataset, new Losses.MSE(), new Optimizers.Default(0.4), 1, 10000, 0.0001).Print();

            Console.WriteLine(net.FeedForward(new double[] { 0.1 })[0]);

            Console.WriteLine(net.FeedForward(new double[] { 1 })[0]);

            Console.WriteLine(net.FeedForward(new double[] { 0.5 })[0]);

            // todo: seems like tehe network is training to out the input values.

            Console.ReadKey();
        }
    }
}
