using System;
using GNet.Extensions;

namespace GNet
{
    class Program
    {
        static void Main()
        {
            Layer[] layers = new Layer[] 
            {
                new Layer(2, new Activations.Identity(), new Initializers.One(), new Initializers.Zero()),
                new Layer(4, new Activations.Tanh(), new Initializers.Uniform(), new Initializers.Uniform()),
                new Layer(1, new Activations.Sigmoid(), new Initializers.Uniform(), new Initializers.Uniform())
            };

            Network net = new Network(layers);
            net.Init();

            Console.WriteLine(net.Validate(Datasets.LogicGates.XOR, new Losses.MSE()));

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            net.Train(Datasets.LogicGates.XOR, new Losses.MSE(), new Optimizers.Default(0.4), 1, 10000000, 0.0001);

            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            Console.WriteLine(net.Validate(Datasets.LogicGates.XOR, new Losses.MSE()));            

            Datasets.LogicGates.XOR.ForEach(D => PrintOutput(net.FeedForward(D.Inputs)));

            Console.ReadKey();

        }

        static void PrintOutput(double[] output)
        {
            foreach (double x in output)
                Console.WriteLine(x);
        }
    }
}
