using System;
using System.Collections.Generic;

namespace PNet
{
    internal class Program
    {
        private static void Main()
        {
            Network net = new Network(new int[] { 2, 2, 1 });
            net.InitializeRandomly(-1, 1);

            List<Data> trainingData = new List<Data>();
            trainingData.Add(new Data(new double[] { 0, 0 }, new double[] { 0 }));
            trainingData.Add(new Data(new double[] { 0, 1 }, new double[] { 1 }));
            trainingData.Add(new Data(new double[] { 1, 0 }, new double[] { 1 }));
            trainingData.Add(new Data(new double[] { 1, 1 }, new double[] { 0 }));

            TrainerMomentum momentumTrainer = new TrainerMomentum(net);

            // todo: sometimes training gets stuck. why?!
            momentumTrainer.Train(trainingData, 5000);

            foreach (double x in net.Output(new double[] { 0, 0 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 0, 1 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 1, 0 }))
                Console.WriteLine(x);

            foreach (double x in net.Output(new double[] { 1, 1 }))
                Console.WriteLine(x);

            Console.ReadKey();
        }
    }
}
