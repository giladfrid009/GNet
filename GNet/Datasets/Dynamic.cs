using GNet.Extensions.Generic;
using GNet.GlobalRandom;
using System;

namespace GNet
{
    public interface IDynamicDataset : IDataset
    {
        void Initialize(int length);

        void Normalize(INormalizer inputNormalizer, INormalizer outputNormalizer);
    }
}

namespace GNet.Datasets.Dynamic
{
    [Serializable]
    public class EvenOdd : IDynamicDataset
    {
        public Data[] DataCollection { get; private set; } = new Data[0];
        public int InputLength { get; }
        public int OutputLength { get; } = 1;
        public int DataLength { get; private set; }

        public EvenOdd(int intputLength)
        {
            InputLength = intputLength;
        }

        private EvenOdd(int inputLength, Data[] dataset) : this(inputLength)
        {
            DataCollection = dataset.Select(D => D);
        }

        public void Initialize(int length)
        {
            DataLength = length;
            DataCollection = new Data[length];

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

                DataCollection[i] = new Data(inputs, new double[] { output });
            }
        }

        public void Normalize(INormalizer inputNormalizer, INormalizer outputNormalizer)
        {
            DataCollection.ForEach(D => D.Normalize(inputNormalizer, outputNormalizer));
        }

        public IDataset Clone() => new EvenOdd(InputLength, DataCollection);
    }

    [Serializable]
    public class Uniform : IDynamicDataset
    {
        public Data[] DataCollection { get; private set; } = new Data[0];
        public int InputLength { get; }
        public int OutputLength { get; }
        public int DataLength { get; private set; }

        public Uniform(int length)
        {
            InputLength = length;
            OutputLength = length;
        }

        private Uniform(int length, Data[] dataset) : this(length)
        {
            DataCollection = dataset.Select(D => D);
        }

        public void Initialize(int length)
        {
            DataLength = length;
            DataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double[] io = new double[InputLength];

                for (int j = 0; j < InputLength; j++)
                {
                    io[j] = GRandom.NextDouble() < 0.5 ? 0.0 : 1.0;
                }

                DataCollection[i] = new Data(io, io.Select(X => X));
            }
        }

        public void Normalize(INormalizer inputNormalizer, INormalizer outputNormalizer)
        {
            DataCollection.ForEach(D => D.Normalize(inputNormalizer, outputNormalizer));
        }

        public IDataset Clone() => new Uniform(InputLength, DataCollection);
    }

    [Serializable]
    public class Func1 : IDynamicDataset
    {
        public Data[] DataCollection { get; private set; } = new Data[0];
        public Func<double, double> IOFunc { get; }
        public double Range { get; }
        public int InputLength { get; } = 1;
        public int OutputLength { get; } = 1;
        public int DataLength { get; private set; }

        public Func1(Func<double, double> ioFunc, double range)
        {
            IOFunc = ioFunc;
            Range = range;
        }

        private Func1(Func<double, double> ioFunc, double range, Data[] dataCollection) : this(ioFunc, range)
        {
            DataCollection = dataCollection.Select(D => D);
        }

        public void Initialize(int length)
        {
            DataLength = length;
            DataCollection = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double num = 0.0;
                double res = 0.0;

                while (res == 0)
                {
                    num = Range * GRandom.NextDouble();
                    res = IOFunc(num);
                }

                DataCollection[i] = new Data(new double[] { num }, new double[] { res });
            }
        }

        public void Normalize(INormalizer inputNormalizer, INormalizer outputNormalizer)
        {
            DataCollection.ForEach(D => D.Normalize(inputNormalizer, outputNormalizer));
        }

        public IDataset Clone() => new Func1(IOFunc, Range, DataCollection);
    }

    [Serializable]
    public class Func2 : IDynamicDataset
    {
        public Data[] DataCollection { get; private set; } = new Data[0];
        public Func<double, double, double> IOFunc { get; }
        public double Range { get; }
        public int InputLength { get; } = 2;
        public int OutputLength { get; } = 1;
        public int DataLength { get; private set; }

        public Func2(Func<double, double, double> ioFunc, double range)
        {
            IOFunc = ioFunc;
            Range = range;
        }

        private Func2(Func<double, double, double> ioFunc, double range, Data[] dataCollection) : this(ioFunc, range)
        {
            DataCollection = dataCollection.Select(D => D);
        }

        public void Initialize(int length)
        {
            DataLength = length;
            DataCollection = new Data[length];

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

                DataCollection[i] = new Data(new double[] { n1, n2 }, new double[] { res });
            }
        }

        public void Normalize(INormalizer inputNormalizer, INormalizer outputNormalizer)
        {
            DataCollection.ForEach(D => D.Normalize(inputNormalizer, outputNormalizer));
        }

        public IDataset Clone() => new Func2(IOFunc, Range, DataCollection);
    }
}
