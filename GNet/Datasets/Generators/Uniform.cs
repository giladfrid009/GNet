using System;

namespace GNet.Datasets.Generators
{
    [Serializable]
    public class Uniform : IDatasetGenerator
    {
        public Shape InputShape { get; }
        public Shape TargetShape { get; }

        public Uniform(Shape dataShape)
        {
            InputShape = dataShape;
            TargetShape = dataShape;
        }

        public Dataset Generate(int length)
        {
            var dataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                var arr = new ImmutableShapedArray<double>(InputShape, () => Utils.GRandom.NextDouble() < 0.5 ? 0.0 : 1.0);

                dataCollection[i] = new Data(arr, arr);
            }

            return new Dataset(ImmutableArray<Data>.FromRef(dataCollection));
        }
    }
}