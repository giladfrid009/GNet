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
            List<LayerData> layersData = new List<LayerData>();
            layersData.Add(new LayerData(2, ConnectionTypes.Dense, ActivationFunctions.Identity, WeightInitializers.He));
            layersData.Add(new LayerData(10, ConnectionTypes.Dense, ActivationFunctions.LeakyReLU, WeightInitializers.He));
            layersData.Add(new LayerData(10, ConnectionTypes.Dense, ActivationFunctions.LeakyReLU, WeightInitializers.He));
            layersData.Add(new LayerData(10, ConnectionTypes.Dense, ActivationFunctions.LeakyReLU, WeightInitializers.He));
            layersData.Add(new LayerData(10, ConnectionTypes.Dense, ActivationFunctions.LeakyReLU, WeightInitializers.He));
            layersData.Add(new LayerData(1, ConnectionTypes.Dense, ActivationFunctions.LeakyReLU, WeightInitializers.Zeroes));

            Network net = new Network(layersData.ToArray());

            List<Data> trainingData = new List<Data>();
            trainingData.Add(new Data(new double[] { 0, 0 }, new double[] { 0 }));
            trainingData.Add(new Data(new double[] { 0, 1 }, new double[] { 1 }));
            trainingData.Add(new Data(new double[] { 1, 0 }, new double[] { 1 }));
            trainingData.Add(new Data(new double[] { 1, 1 }, new double[] { 0 }));

            TrainerClassic trainer = new TrainerClassic(net);

            trainer.Train(trainingData, 1);

            foreach (double x in net.Output(new double[] { 0, 0 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 0, 1 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 1, 0 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 1, 1 }))
                Console.WriteLine(x);

            Console.WriteLine(net.Validate(trainingData, new double[] { 0.05 }));

            Console.ReadKey();
        }        
    }
}
