using System;

namespace GNet.Datasets.Generators
{
    [Serializable]
    public class Func2 : IDatasetGenerator
    {
        public Func<double, double, double> Func { get; }
        public Shape InputShape { get; } = new Shape(2);
        public Shape TargetShape { get; } = new Shape(1);

        public Func2(Func<double, double, double> func)
        {
            Func = func;
        }

        public Dataset Generate(int length)
        {
            var dataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double n1 = 0.0;
                double n2 = 0.0;
                double res = 0.0;

                while (res == 0.0)
                {
                    n1 = Utils.GRandom.NextDouble();
                    n2 = Utils.GRandom.NextDouble();
                    res = Func(n1, n2);
                }

                dataCollection[i] = new Data(new ImmutableShapedArray<double>(n1, n2), new ImmutableShapedArray<double>(res));
            }

            return new Dataset(ImmutableArray<Data>.FromRef(dataCollection));
        }
    }
}