using System;

namespace GNet.Datasets.Generators
{
    [Serializable]
    public class Func2 : IDatasetGenerator
    {
        public Func<double, double, double> IOFunc { get; }
        public Shape InputShape { get; } = new Shape(2);
        public Shape OutputShape { get; } = new Shape(1);
        public double Range { get; }

        public Func2(Func<double, double, double> ioFunc, double range)
        {
            IOFunc = ioFunc;
            Range = range;
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
                    n1 = Range * GRandom.NextDouble();
                    n2 = Range * GRandom.NextDouble();
                    res = IOFunc(n1, n2);
                }

                dataCollection[i] = new Data(
                    new ShapedArrayImmutable<double>(new Shape(2), n1, n2),
                    new ShapedArrayImmutable<double>(new Shape(1), res));
            }

            return new Dataset(dataCollection);
        }

        public IDatasetGenerator Clone()
        {
            return new Func2(IOFunc, Range);
        }
    }
}