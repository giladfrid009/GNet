using System;

namespace GNet.Datasets.Generators
{
    [Serializable]
    public class Func1 : IDatasetGenerator
    {
        public Func<double, double> IOFunc { get; }
        public double Range { get; }

        public Func1(Func<double, double> ioFunc, double range)
        {
            IOFunc = ioFunc;
            Range = range;
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
                    num = Range * GRandom.NextDouble();
                    res = IOFunc(num);
                }

                dataCollection[i] = new Data(
                    new ShapedArrayImmutable<double>(new Shape(1), num),
                    new ShapedArrayImmutable<double>(new Shape(1), res));
            }

            return new Dataset(dataCollection);
        }

        public IDatasetGenerator Clone()
        {
            return new Func1(IOFunc, Range);
        }
    }
}