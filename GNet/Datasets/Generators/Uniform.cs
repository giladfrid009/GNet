using System;

namespace GNet.Datasets.Generators
{
    [Serializable]
    public class Uniform : IDatasetGenerator
    {
        public Shape InputShape { get; }
        public Shape OutputShape { get; }

        public Uniform(Shape ioShape)
        {
            InputShape = ioShape;
            OutputShape = ioShape;
        }

        public Dataset Generate(int length)
        {
            var dataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                var io = new ShapedArrayImmutable<double>(InputShape, () => GRandom.NextDouble() < 0.5 ? 0.0 : 1.0);

                dataCollection[i] = new Data(io, io);
            }

            return new Dataset(dataCollection);
        }
    }
}