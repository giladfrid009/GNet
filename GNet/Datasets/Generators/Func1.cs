using System;

namespace GNet.Datasets.Generators
{
    [Serializable]
    public class Func1 : IDatasetGenerator
    {
        public Func<double, double> Func { get; }
        public Shape InputShape { get; } = new Shape(1);
        public Shape TargetShape { get; } = new Shape(1);

        public Func1(Func<double, double> func)
        {
            Func = func;
        }

        public Dataset Generate(int length)
        {
            var dataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double num = 0.0;
                double res = 0.0;

                while (res == 0)
                {
                    num = Utils.GRandom.NextDouble();
                    res = Func(num);
                }

                dataCollection[i] = new Data(new ImmutableShapedArray<double>(num), new ImmutableShapedArray<double>(res));
            }

            return new Dataset(ImmutableArray<Data>.FromRef(dataCollection));
        }
    }
}