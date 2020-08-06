using GNet.Utils;
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
            return new Dataset(new ImmutableArray<Data>(length, () =>
            {
                var arr = new ImmutableShapedArray<double>(InputShape, () => GRandom.Uniform() < 0.5 ? 0.0 : 1.0);

                return new Data(arr, arr);
            }));
        }
    }
}