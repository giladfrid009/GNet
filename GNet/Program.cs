using System;
using System.Collections.Generic;
using GNet.Trainers;

namespace GNet
{
    internal class Program
    {
        private static void Main()
        {
            List<LayerConfig> layersConfig = new List<LayerConfig>();
            layersConfig.Add(new LayerConfig(2, Activations.Identity, Initializers.Zero, Initializers.Zero));
            layersConfig.Add(new LayerConfig(4, Activations.ELU, Initializers.HeNormal, Initializers.SmallConst));
            layersConfig.Add(new LayerConfig(10, Activations.ELU, Initializers.HeNormal, Initializers.SmallConst));
            layersConfig.Add(new LayerConfig(5, Activations.ELU, Initializers.HeNormal, Initializers.SmallConst));
            layersConfig.Add(new LayerConfig(1, Activations.Sigmoid, Initializers.Normal, Initializers.Normal));

            Network net = new Network(layersConfig.ToArray());

            var trainingData = Datasets.LogicGates.XOR;

            Console.WriteLine(net.Validate(trainingData, Losses.Squared) + "\n");

            TrainerMomentum trainer = new TrainerMomentum(net, Losses.Squared);

            trainer.Train(trainingData, 1, 50000, 0.002);

            foreach (double x in net.Output(new double[] { 0, 0 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 0, 1 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 1, 0 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 1, 1 }))
                Console.WriteLine(x);

            Console.WriteLine("\n" + net.Validate(trainingData, Losses.Absolute));

            Console.ReadKey();
        }        
    }
}
