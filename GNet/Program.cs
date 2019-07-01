using System;
using System.Linq;

namespace GNet
{
    internal class Program
    {
        private static void Main()
        {
            var MNISTImages = IDXReader.Reader.ReadFileND<byte[,]>(@"D:\Users\Gilad\Documents\MNIST\train-images.idx3-ubyte", false).ToArray();

            var MNISTLabels = IDXReader.Reader.ReadFile1D<byte>(@"D:\Users\Gilad\Documents\MNIST\train-labels.idx1-ubyte", false).ToArray(); 

            Data[] dataCollection = new Data[MNISTImages.Length];

            for (int i = 0; i < MNISTImages.Length; i++)
            {
                dataCollection[i] = new Data(MNISTImages[i], new double[] { MNISTLabels[i] }, new Normalizers.Division(255), new Normalizers.Division(255));
            }

            Datasets.Custom MNIST = new Datasets.Custom(dataCollection);

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

            net.Train(trainingDataset, new Losses.MSE(), new Optimizers.NestrovMomentum(), 30, 1000, 0.01, validationDataset, validationLoss).Print();

            Console.ReadKey();

        }
    }
}
