using System;
using System.Collections.Generic;
using GNet.Trainers;

namespace GNet
{
    internal class Program
    {
        private static void Main()
        { 
            // todo: fix all. some activations dont work well as output activations.
            // todo: layer's weights are it's inputs, not outputs. find write way to write connectionType and weightInitializer, and redo weights array.
            List<LayerConfig> layersConfig = new List<LayerConfig>();
            layersConfig.Add(new LayerConfig(2, Connections.Dense, Activations.Identity, Initializers.Zero, Initializers.Zero));
            layersConfig.Add(new LayerConfig(10, Connections.Dense, Activations.LeCunTanh, Initializers.LeCunNormal, Initializers.LeCunUniform));
            layersConfig.Add(new LayerConfig(1, Connections.Dense, Activations.Sigmoid, Initializers.Normal, Initializers.LeCunUniform));

            Network net = new Network(layersConfig.ToArray());

            List<Data> trainingData = new List<Data>();
            trainingData.Add(new Data(new double[] { 0, 0 }, new double[] { 0 }));
            trainingData.Add(new Data(new double[] { 0, 1 }, new double[] { 1 }));
            trainingData.Add(new Data(new double[] { 1, 0 }, new double[] { 1 }));
            trainingData.Add(new Data(new double[] { 1, 1 }, new double[] { 0 }));


            // todo: only squared loss works. why :c
            TrainerMomentum trainer = new TrainerMomentum(net, Losses.Squared);

            Console.WriteLine(net.Validate(trainingData, Losses.Squared));

            trainer.Train(trainingData, 1, 500000, 0.0002);

            foreach (double x in net.Output(new double[] { 0, 0 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 0, 1 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 1, 0 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 1, 1 }))
                Console.WriteLine(x);

            Console.WriteLine(net.Validate(trainingData, Losses.Squared));

            Console.ReadKey();
        }        
    }
}
