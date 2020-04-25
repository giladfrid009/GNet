using System;

namespace GNet.Datasets.Generators
{
    [Serializable]
    public class EvenOdd : IDatasetGenerator
    {
        public Shape InputShape { get; }
        public Shape TargetShape { get; } = new Shape(1);

        public EvenOdd(Shape inputShape)
        {
            InputShape = inputShape;
        }

        public Dataset Generate(int length)
        {
            var dataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                var input = new ImmutableShapedArray<double>(InputShape, () => Utils.GRandom.NextDouble() < 0.5 ? 0.0 : 1.0);

                int zeroCount = 0;

                input.ForEach(X =>
                {
                    if (X == 0)
                    {
                        zeroCount++;
                    }
                });

                bool isEven = zeroCount % 2 == 0;

                dataCollection[i] = new Data(input, new ImmutableShapedArray<double>(isEven ? 1.0 : 0.0));
            }

            return new Dataset(ImmutableArray<Data>.FromRef(dataCollection));
        }
    }
}