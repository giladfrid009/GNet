using GNet.Utils;
using System;
using NCollections;

namespace GNet.Datasets.Generators
{
    [Serializable]
    public class EvenOdd : IDatasetGenerator
    {
        public Shape InputShape { get; }
        public Shape TargetShape { get; }
        public bool IsBinary { get; }

        public EvenOdd(Shape inputShape, bool isBinary)
        {
            InputShape = inputShape;
            IsBinary = isBinary;

            TargetShape = isBinary ? new Shape(2) : new Shape(1);
        }

        public Dataset Generate(int length)
        {
            Data[] dataArray = new Data[length];

            for (int i = 0; i < length; i++)
            {
                var input = new Tensor<double>(InputShape, () => GRandom.Uniform() < 0.5 ? 0.0 : 1.0);

                int oneCount = 0;

                input.ForEach(X => oneCount += X == 1.0 ? 1 : 0);

                bool isEven = oneCount % 2 == 0;

                double[] outArr;

                if (IsBinary)
                {
                    outArr = isEven ? new double[] { 1.0, 0.0 } : new double[] { 0.0, 1.0 };
                }
                else
                {
                    outArr = isEven ? new double[] { 1.0 } : new double[] { 0.0 };
                }

                dataArray[i] = new Data(input, Tensor<double>.FromRef(TargetShape, outArr));
            }

            return Dataset.FromRef(dataArray);
        }
    }
}