using System;
using GNet.Extensions.Misc;

namespace GNet
{
    internal class Program
    {
        private static void Main()
        {
            Layer[] layers = new Layer[]
            {
                new Layer(10, new Activations.Identity(), new Initializers.One(), new Initializers.Zero()),
                new Layer(10, new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero()),
                new Layer(1, new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Zero())
            };

            Network net = new Network(layers);
            net.Init();

            var trainingDataset = new Datasets.Dynamic.EvenOdd(10);
            var validationDataset = new Datasets.Dynamic.EvenOdd(10);
            var validationLoss = new OutTransformers.Losses.BinaryRoundLoss();

            trainingDataset.Generate(2000);
            validationDataset.Generate(1000);

            net.Validate(trainingDataset, validationLoss).Print();

            net.Train(trainingDataset, new Losses.MSE(), new Optimizers.Adam(), 5, 1000, 0.01, validationLoss).Print();                       

            net.Validate(validationDataset, validationLoss).Print();

            Console.ReadKey();
        }
    }
}
