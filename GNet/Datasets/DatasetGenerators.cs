using System;
using GNet.GlobalRandom;

namespace GNet
{
    public interface IDatasetGenerator : ICloneable<IDatasetGenerator>
    {
        Dataset Generate(int length);
    }
}

namespace GNet.Datasets.Generators
{
    [Serializable]
    public class EvenOdd : IDatasetGenerator
    {
        public int InputLength { get; }

        public EvenOdd(int inputLength)
        {
            InputLength = inputLength;
        }

        public Dataset Generate(int length)
        {
            Data[] dataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                int zeroCount = 0;
                double[] inputs = new double[InputLength];

                for (int j = 0; j < InputLength; j++)
                {
                    inputs[j] = GRandom.NextDouble() < 0.5 ? 0.0 : 1.0;
                    zeroCount += inputs[j] == 0.0 ? 0 : 1;
                }

                double output = zeroCount % 2 == 0 ? 0.0 : 1.0;

                dataCollection[i] = new Data(inputs, new double[] { output });
            }

            return new Dataset(dataCollection);
        }

        public IDatasetGenerator Clone() => new EvenOdd(InputLength);
    }

    [Serializable]
    public class Uniform : IDatasetGenerator
    {
        public int IOLength { get; }

        public Uniform(int ioLength)
        {
            IOLength = ioLength;
        }

        public Dataset Generate(int length)
        {
            Data[] dataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double[] io = new double[IOLength];

                for (int j = 0; j < IOLength; j++)
                {
                    io[j] = GRandom.NextDouble() < 0.5 ? 0.0 : 1.0;
                }

                dataCollection[i] = new Data(io, io);
            }

            return new Dataset(dataCollection);
        }

        public IDatasetGenerator Clone() => new Uniform(IOLength);
    }

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
            Data[] dataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double num = 0.0;
                double res = 0.0;

                while (res == 0)
                {
                    num = Range * GRandom.NextDouble();
                    res = IOFunc(num);
                }

                dataCollection[i] = new Data(new double[] { num }, new double[] { res });
            }

            return new Dataset(dataCollection);
        }

        public IDatasetGenerator Clone() => new Func1(IOFunc, Range);
    }

    [Serializable]
    public class Func2 : IDatasetGenerator
    {
        public Func<double, double, double> IOFunc { get; }
        public double Range { get; }

        public Func2(Func<double, double, double> ioFunc, double range)
        {
            IOFunc = ioFunc;
            Range = range;
        }

        public Dataset Generate(int length)
        {
            Data[] dataCollection = new Data[length];

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

                dataCollection[i] = new Data(new double[] { n1, n2 }, new double[] { res });
            }

            return new Dataset(dataCollection);
        }

        public IDatasetGenerator Clone() => new Func2(IOFunc, Range);
    }
}
