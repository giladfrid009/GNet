using System;
using GNet.Extensions;
using GNet.GlobalRandom;

namespace GNet.Datasets
{
    public interface IDynamicDataset : IDataset
    {
        Data[] Generate(int length, INormalizer inputNormalizer, INormalizer outputNormalizer);
    }
}

namespace GNet.Datasets.Dynamic
{
    public class EvenOdd : IDynamicDataset
    {
        public int Length { get { return Dataset.Length; } }
        public int InputLength { get; }
        public int OutputLength { get; } = 1;
        public Data[] Dataset { get; private set; } = new Data[0];

        public EvenOdd(int intputLength)
        {
            InputLength = intputLength;
        }

        private EvenOdd(int inputLength, Data[] dataset) : this(inputLength)
        {
            Dataset = dataset.Select(D => D);
        }

        public Data[] Generate(int length, INormalizer inputNormalizer = null, INormalizer outputNormalizer = null)
        {
            Dataset = new Data[length];

            for (int i = 0; i < length; i++)
            {
                int count = 0;
                double[] inputs = new double[InputLength];

                for (int j = 0; j < InputLength; j++)
                {
                    inputs[j] = GRandom.NextDouble() < 0.5 ? 0 : 1;
                    count += inputs[j] == 0 ? 0 : 1;
                }

                double output = count % 2 == 0 ? 0 : 1;

                Dataset[i] = new Data(inputs, new double[] { output }, inputNormalizer, outputNormalizer);
            }

            return Dataset;
        }

        public IDataset Clone() => new EvenOdd(InputLength, Dataset);
    }

    public class Uniform : IDynamicDataset
    {
        public int Length { get { return Dataset.Length; } }
        public int InputLength { get; }
        public int OutputLength { get; }
        public Data[] Dataset { get; private set; } = new Data[0];

        public Uniform(int IOLength)
        {
            InputLength = IOLength;
            OutputLength = IOLength;
        }

        private Uniform(int IOLength, Data[] dataset): this(IOLength)
        {
            Dataset = dataset.Select(D => D);
        }

        public Data[] Generate(int length, INormalizer inputNormalizer = null, INormalizer outputNormalizer = null)
        {
            Dataset = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double[] io = new double[InputLength];

                for (int j = 0; j < InputLength; j++)
                {
                    io[j] = GRandom.NextDouble() < 0.5 ? 0 : 1;
                }

                Dataset[i] = new Data(io, io.Select(X => X), inputNormalizer, outputNormalizer);
            }

            return Dataset;
        }

        public IDataset Clone() => new Uniform(InputLength, Dataset);
    }

    public class MathOp1 : IDynamicDataset
    {
        public enum Ops1 { Sin, Cos, Tan, Exp, Ln, Abs, Asin, Acos, Atan, Round }

        public int Length { get { return Dataset.Length; } }
        public int InputLength { get; } = 1;
        public int OutputLength { get; } = 1;
        public Data[] Dataset { get; private set; } = new Data[0];

        public double Range { get; }
        public Ops1 Operation { get; }

        private Func<double, double> mathFunc;

        public MathOp1(Ops1 operation, double range)
        {
            Operation = operation;
            Range = range;

            switch (operation)
            {
                case Ops1.Sin: mathFunc = (X) => Math.Sin(X); break;

                case Ops1.Cos: mathFunc = (X) => Math.Cos(X); break;

                case Ops1.Tan: mathFunc = (X) => Math.Tan(X); break;

                case Ops1.Exp: mathFunc = (X) => Math.Exp(X); break;

                case Ops1.Ln: mathFunc = (X) => Math.Log10(X); break;

                case Ops1.Abs: mathFunc = (X) => Math.Abs(X); break;

                case Ops1.Asin: mathFunc = (X) => Math.Asin(X); break;

                case Ops1.Acos: mathFunc = (X) => Math.Acos(X); break;

                case Ops1.Atan: mathFunc = (X) => Math.Atan(X); break;

                case Ops1.Round: mathFunc = (X) => Math.Round(X); break;

                default: throw new ArgumentOutOfRangeException("Unsupported operation");
            }
        }

        public MathOp1(Ops1 operation, double range, Data[] dataset) : this(operation, range)
        {
            Dataset = dataset.Select(D => D);
        }

        public Data[] Generate(int length, INormalizer inputNormalizer, INormalizer outputNormalizer)
        {
            Dataset = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double num = 0.0;
                double res = 0.0;

                while (res == 0)
                {
                    num = Range * GRandom.NextDouble();
                    res = mathFunc(num);
                }

                Dataset[i] = new Data(new double[] { num }, new double[] { res }, inputNormalizer, outputNormalizer);
            }

            return Dataset;
        }

        public IDataset Clone() => new MathOp1(Operation, Range, Dataset);
    }

    public class MathOp2 : IDynamicDataset
    {
        public enum Ops2 { Add, Sub, Mul, Div, Rem, Pow, Root, Log, Min, Max }

        public int Length { get { return Dataset.Length; } }
        public int InputLength { get; } = 2;
        public int OutputLength { get; } = 1;
        public Data[] Dataset { get; private set; } = new Data[0];

        public double Range { get; }
        public Ops2 Operation { get; }

        private Func<double, double, double> mathFunc;

        public MathOp2(Ops2 operation, double range)
        {
            Operation = operation;
            Range = range;

            switch (operation)
            {
                case Ops2.Add: mathFunc = (X, Y) => X + Y; break;

                case Ops2.Sub: mathFunc = (X, Y) => X - Y; break;

                case Ops2.Mul: mathFunc = (X, Y) => X * Y; break;

                case Ops2.Div: mathFunc = (X, Y) => X / Y; break;

                case Ops2.Rem: mathFunc = (X, Y) => X % Y; break;

                case Ops2.Pow: mathFunc = (X, Y) => Math.Pow(X, Y); break;

                case Ops2.Root: mathFunc = (X, Y) => Math.Pow(X, 1.0 / Y); break;

                case Ops2.Log: mathFunc = (X, Y) => Math.Log(X, Y); break;

                case Ops2.Min: mathFunc = (X, Y) => Math.Min(X, Y); break;

                case Ops2.Max: mathFunc = (X, Y) => Math.Max(X, Y); break;

                default: throw new ArgumentOutOfRangeException("Unsupported operation");
            }
        }

        private MathOp2(Ops2 operation, double range, Data[] dataset) : this(operation, range)
        {
            Dataset = dataset.Select(D => D);
        }

        public Data[] Generate(int length, INormalizer inputNormalizer, INormalizer outputNormalizer)
        {
            Dataset = new Data[length];

            for (int i = 0; i < length; i++)
            {
                double n1 = 0.0;
                double n2 = 0.0;
                double res = 0.0;

                while (res == 0)
                {
                    n1 = Range * GRandom.NextDouble();
                    n2 = Range * GRandom.NextDouble();
                    res = mathFunc(n1, n2);
                }

                Dataset[i] = new Data(new double[] { n1, n2 }, new double[] { res }, inputNormalizer, outputNormalizer);
            }

            return Dataset;
        }

        public IDataset Clone() => new MathOp2(Operation, Range, Dataset);
    }    
}
