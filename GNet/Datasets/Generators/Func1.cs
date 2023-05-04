using GNet.Utils;
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
            Data[] dataArray = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double num = 0.0;
                double res = 0.0;

                while (res == 0)
                {
                    num = GRandom.Uniform();
                    res = Func(num);
                }

                dataArray[i] = new Data(new ShapedArray<double>(num), new ShapedArray<double>(res));
            }

            return Dataset.FromRef(dataArray);
        }
    }
}