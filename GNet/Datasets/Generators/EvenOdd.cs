using System;

namespace GNet.Datasets.Generators
{
    [Serializable]
    public class EvenOdd : IDatasetGenerator
    {
        public int InputLength { get; }

        public EvenOdd(int inputLength)
        {
            InputLength = inputLength;
        }

        public Dataset Generate(int length)
        {
            var dataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                int zeroCount = 0;
                double[] inputs = new double[InputLength];

                for (int j = 0; j < InputLength; j++)
                {
                    inputs[j] = GRandom.NextDouble() < 0.5 ? 0.0 : 1.0;
                    zeroCount += inputs[j] == 0.0 ? 0 : 1;
                }

                double output = zeroCount % 2 == 0 ? 0.0 : 1.0;

                dataCollection[i] = new Data(
                    new ShapedArrayImmutable<double>(new Shape(inputs.Length), inputs),
                    new ShapedArrayImmutable<double>(new Shape(1), output));
            }

            return new Dataset(dataCollection);
        }

        public IDatasetGenerator Clone()
        {
            return new EvenOdd(InputLength);
        }
    }
}