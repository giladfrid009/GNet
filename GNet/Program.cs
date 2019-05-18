using System;
using GNet.Trainers;

namespace GNet
{
    internal class Program
    {
        private static void Main()
        {
            LayerConfig[] layersConfig = new LayerConfig[]
            {
                new LayerConfig(2, new Activations.Identity(), new Initializers.Zero(), new Initializers.Zero()),
                new LayerConfig(4, new Activations.ELU(), new Initializers.Normal(), new Initializers.Zero()),
                new LayerConfig(10, new Activations.ELU(), new Initializers.Normal(), new Initializers.Zero()),
                new LayerConfig(5, new Activations.ELU(), new Initializers.Normal(), new Initializers.Zero()),
                new LayerConfig(1, new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Zero())
            };

            Network net = new Network(layersConfig);

            var trainingData = Datasets.LogicGates.XOR;

            var outp = net.Output(trainingData[0].Inputs);

            var loss = net.Validate(trainingData, new Losses.MSE());

            Console.WriteLine(net.Validate(trainingData, new Losses.MSE()) + "\n");

            TrainerClassic trainer = new TrainerClassic(net, new Losses.MSE());

            trainer.Train(trainingData, 1, true, 10000);

            foreach (double x in net.Output(new double[] { 0, 0 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 0, 1 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 1, 0 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 1, 1 }))
                Console.WriteLine(x);

            Console.WriteLine("\n" + net.Validate(trainingData, new Losses.MSE()));

            Console.ReadKey();
        }        
    }
}
