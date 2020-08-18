using GNet.Utils;
using System;
using NCollections;

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
            Data[] dataArray = new Data[length];

            for (int i = 0; i < length; i++)
            {
                var arr = new Tensor<double>(InputShape, () => GRandom.Uniform() < 0.5 ? 0.0 : 1.0);

                dataArray[i] = new Data(arr, arr);
            }

            return Dataset.FromRef(dataArray);
        }
    }
}