using System;

namespace GNet
{
    class Program
    {
        static void Main()
        {
            Layer[] layers = new Layer[] 
            {
                new Layer(2, new Activations.Identity(), new Initializers.One(), new Initializers.Zero()),
                new Layer(6, new Activations.Tanh(), new Initializers.Uniform(), new Initializers.Uniform()),
                new Layer(1, new Activations.Sigmoid(), new Initializers.Uniform(), new Initializers.Uniform())
            };

            Network net = new Network(layers);
            net.Init();

            var dataset = Datasets.MathOperations.GenerateDataset(Datasets.MathOperations.Ops.Add, 1, 1000);

            Console.WriteLine(net.Validate(Datasets.LogicGates.XOR, new Losses.MSE()));

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            net.Train(dataset, new Losses.MSE(), new Optimizers.Default(0.4), 5, 1000, 0.0001);

            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            Console.WriteLine(net.Validate(Datasets.LogicGates.XOR, new Losses.MSE()));

            Console.ReadKey();

        }
    }
}
