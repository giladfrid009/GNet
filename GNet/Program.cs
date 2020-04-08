using System;
using System.Diagnostics;

namespace GNet
{
    public class Program
    {
        private static void Main()
        {
            var generator = new Datasets.Generators.Func2((X, Y) => X * Y, 1);

            var tDataset = generator.Generate(1000);
            var vDataset = generator.Generate(100);

            Network net = new Network
            (
                new Layers.Dense(new Shape(2), new Activations.Identity(), new Initializers.Normal(), new Initializers.Zero()),
                new Layers.Dense(new Shape(5), new Activations.Tanh(), new Initializers.Normal(), new Initializers.Zero()),
                new Layers.Dense(new Shape(5), new Activations.Tanh(), new Initializers.Normal(), new Initializers.Zero()),
                new Layers.Dense(new Shape(1), new Activations.Tanh(), new Initializers.Normal(), new Initializers.Zero())
            );

            using (Logger log = new Logger(net))
            {
                net.Train(tDataset, new Losses.MSE(), new Optimizers.Default(), 1, 1000, 0.01, vDataset, new Losses.MAE());
            }

            Console.ReadKey();
        }
    }
}