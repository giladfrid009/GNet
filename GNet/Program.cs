using System;
using System.Collections.Generic;
using GNet.Trainers;

namespace GNet
{
    internal class Program
    {
        private static void Main()
        { 
            // todo: fix all. 
            // todo: devision by 0 when relu is happening, hence the Nan. also tanh doesnt behave well compared to sigmoid
            // todo: layer's weights are it's inputs, not outputs. find write way to write connectionType and weightInitializer, and redo weights array.
            List<LayerConfig> layersConfig = new List<LayerConfig>();
            layersConfig.Add(new LayerConfig(2, Connections.Dense, Activations.Identity, Initializers.Gaussian, Initializers.Zero));
            layersConfig.Add(new LayerConfig(10, Connections.Dense, Activations.LeakyReLU, Initializers.XavierNormal, Initializers.Gaussian));
            layersConfig.Add(new LayerConfig(10, Connections.Dense, Activations.LeakyReLU, Initializers.XavierNormal, Initializers.Gaussian));
            layersConfig.Add(new LayerConfig(1, Connections.Dense, Activations.Sigmoid, Initializers.Zero, Initializers.Gaussian));

            Network net = new Network(layersConfig.ToArray());

            List<Data> trainingData = new List<Data>();
            trainingData.Add(new Data(new double[] { 0, 0 }, new double[] { 0 }));
            trainingData.Add(new Data(new double[] { 0, 1 }, new double[] { 1 }));
            trainingData.Add(new Data(new double[] { 1, 0 }, new double[] { 1 }));
            trainingData.Add(new Data(new double[] { 1, 1 }, new double[] { 0 }));


            // todo: only squared loss works. why :c
            TrainerMomentum trainer = new TrainerMomentum(net, Losses.Squared);

            Console.WriteLine(net.Validate(trainingData, Losses.Absolute));

            trainer.Train(trainingData, 1, 500000, 0.0002);

            foreach (double x in net.Output(new double[] { 0, 0 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 0, 1 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 1, 0 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 1, 1 }))
                Console.WriteLine(x);

            Console.WriteLine(net.Validate(trainingData, Losses.Absolute));

            Console.ReadKey();
        }        
    }
}
