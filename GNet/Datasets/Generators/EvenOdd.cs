using System;

namespace GNet.Datasets.Generators
{
    [Serializable]
    public class EvenOdd : IDatasetGenerator
    {
        public Shape InputShape { get; }
        public Shape OutputShape { get; } = new Shape(1);

        public EvenOdd(Shape inputShape)
        {
            InputShape = inputShape;
        }

        public Dataset Generate(int length)
        {
            var dataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                var input = new ShapedArrayImmutable<double>(InputShape, () => Utils.GRandom.NextDouble() < 0.5 ? 0.0 : 1.0);

                int zeroCount = 0;

                input.ForEach(X =>
                {
                    if (X == 0)
                    {
                        zeroCount++;
                    }
                });

                double output = zeroCount % 2 == 0 ? 0.0 : 1.0;

                dataCollection[i] = new Data(input, new ShapedArrayImmutable<double>(new Shape(1), output));
            }

            return new Dataset(dataCollection);
        }
    }
}