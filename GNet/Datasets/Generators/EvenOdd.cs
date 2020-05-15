using GNet.Utils;
using System;

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
            var dataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                var input = new ImmutableShapedArray<double>(InputShape, () => GRandom.Uniform() < 0.5 ? 0.0 : 1.0);

                int zeroCount = 0;

                input.ForEach(X =>
                {
                    if (X == 0)
                    {
                        zeroCount++;
                    }
                });

                bool isEven = zeroCount % 2 == 0;

                double[] outArr;

                if (IsBinary)
                {
                    outArr = isEven ? new double[] { 1.0, 0.0 } : new double[] { 0.0, 1.0 };
                }
                else
                {
                    outArr = isEven ? new double[] { 1.0 } : new double[] { 0.0 };
                }

                dataCollection[i] = new Data(input, ImmutableShapedArray<double>.FromRef(TargetShape, outArr));
            }

            return new Dataset(ImmutableArray<Data>.FromRef(dataCollection));
        }
    }
}