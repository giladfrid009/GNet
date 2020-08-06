using GNet.Utils;
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
            return new Dataset(new ImmutableArray<Data>(length, () =>
            {
                double n1 = 0.0;
                double n2 = 0.0;
                double res = 0.0;

                while (res == 0.0)
                {
                    n1 = GRandom.Uniform();
                    n2 = GRandom.Uniform();
                    res = Func(n1, n2);
                }

                return new Data(new ImmutableShapedArray<double>(n1, n2), new ImmutableShapedArray<double>(res));
            }));
        }
    }
}