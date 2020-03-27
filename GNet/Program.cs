﻿using System;

namespace GNet
{
    public class Program
    {
        private static void Main()
        {
            var net = new Network
            (
                new Layers.Dense(new Shape(30, 30), new Activations.Identity(), new Initializers.Zero(), new Initializers.Zero()),
                new Layers.Convolutional
                (
                    new Shape(30, 30),
                    new Shape(4, 4),
                    new ArrayImmutable<int>(2, 2),
                    new ArrayImmutable<int>(0, 0),
                    20,
                    new Activations.Tanh(),
                    new Initializers.Normal(),
                    new Initializers.Zero()
                ),
                new Layers.Pooling
                (
                    new Shape(20, 14, 14),
                    new Shape(1, 4, 4),
                    new ArrayImmutable<int>(1, 2, 2),
                    new ArrayImmutable<int>(0, 0, 0),
                    new Layers.Poolers.Max()
                ),
                new Layers.Dense(new Shape(100), new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero()),
                new Layers.Dense(new Shape(30), new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero()),
                new Layers.Dense(new Shape(1), new Activations.Tanh(), new Initializers.LeCunNormal(), new Initializers.Zero())
            );

            var datasetGenerator = new Datasets.Generators.EvenOdd(new Shape(30, 30));
            Dataset trainingDataset = datasetGenerator.Generate(600);
            Dataset validationDataset = datasetGenerator.Generate(100);

            net.Initialize();

            using (new Logger(net) { LogBatches = true })
            {
                net.Train(trainingDataset, new Losses.MSE(), new Optimizers.NestrovMomentum(), 20, 1000, 0.001, validationDataset, new OutTransformers.Losses.BinaryRoundLoss(), false);
            }

            Console.ReadKey();
        }
    }
}