using System;
using GNet.Extensions;

namespace GNet
{
    internal class Program
    {
        // todo: when doing math ops int/int + double * double for example, ints doesnt get converted, meaning int/int will be rounded
        // todo: check all computations for possible {(int) op (int)} cases.

        private static void Main()
        {
            Layer[] layers = new Layer[]
            {
                new Layer(10, new Activations.Identity(), new Initializers.One(), new Initializers.Zero()),
                new Layer(8, new Activations.HardSigmoid(), new Initializers.Normal(), new Initializers.Zero()),
                new Layer(6, new Activations.HardSigmoid(), new Initializers.Normal(), new Initializers.Zero()),
                new Layer(3, new Activations.HardSigmoid(), new Initializers.Normal(), new Initializers.Zero()),
                new Layer(1, new Activations.Sigmoid(), new Initializers.Normal(), new Initializers.Zero())
            };

            Network net = new Network(layers);
            net.Init();

            var trainingDataset = new Datasets.Dynamic.EvenOdd(10);
            trainingDataset.Generate(2000);

            net.Validate(trainingDataset, new Losses.MSE()).Print();

            var validationDataset = new Datasets.Dynamic.EvenOdd(10);
            validationDataset.Generate(1000);

            net.Train(trainingDataset, new Losses.MSE(), new Optimizers.AdaMax(), 5, 1000, 0.01).Print();            
            

            net.Validate(validationDataset, new Losses.MSE()).Print();

            Console.ReadKey();
        }
    }
}
