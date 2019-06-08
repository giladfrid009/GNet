using GNet.GlobalRandom;
using System;

namespace GNet.Datasets
{
    internal class MathOperations
    {
        public enum Ops1 { Sin, Cos, Tan, Exp, Ln, Abs, Asin, Acos, Atan, Round }
        public enum Ops2 { Add, Sub, Mul, Div, Rem, Pow, Root, Log, Min, Max }

        public static Data[] GenerateDataset(Ops1 op1, double scale, int datasetLength, INormalizer normalizer = null)
        {
            Func<double, double> operation;

            switch (op1)
            {
                case Ops1.Sin: operation = (X) => Math.Sin(X); break;

                case Ops1.Cos: operation = (X) => Math.Cos(X); break;

                case Ops1.Tan: operation = (X) => Math.Tan(X); break;

                case Ops1.Exp: operation = (X) => Math.Exp(X); break;

                case Ops1.Ln: operation = (X) => Math.Log10(X); break;

                case Ops1.Abs: operation = (X) => Math.Abs(X); break;

                case Ops1.Asin: operation = (X) => Math.Asin(X); break;

                case Ops1.Acos: operation = (X) => Math.Acos(X); break;

                case Ops1.Atan: operation = (X) => Math.Atan(X); break;

                case Ops1.Round: operation = (X) => Math.Round(X); break;

                default: throw new ArgumentOutOfRangeException("Unsupported operation");
            }

            Data[] dataSet = new Data[datasetLength];

            for (int i = 0; i < datasetLength; i++)
            {
                double num = 0.0;
                double res = 0.0;

                while (res == 0)
                {
                    num = scale * GRandom.NextDouble();
                    res = operation(num);
                }

                dataSet[i] = new Data(new double[] { num }, new double[] { res }, normalizer);
            }

            return dataSet;
        }

        public static Data[] GenerateDataset(Ops2 op2, double scale, int datasetLength, INormalizer normalizer = null)
        {
            Func<double, double, double> operation;

            switch (op2)
            {
                case Ops2.Add: operation = (X, Y) => X + Y; break;

                case Ops2.Sub: operation = (X, Y) => X - Y; break;

                case Ops2.Mul: operation = (X, Y) => X * Y; break;

                case Ops2.Div: operation = (X, Y) => X / Y; break;

                case Ops2.Rem: operation = (X, Y) => X % Y; break;

                case Ops2.Pow: operation = (X, Y) => Math.Pow(X, Y); break;

                case Ops2.Root: operation = (X, Y) => Math.Pow(X, 1.0 / Y); break;

                case Ops2.Log: operation = (X, Y) => Math.Log(X, Y); break;

                case Ops2.Min: operation = (X, Y) => Math.Min(X, Y); break;

                case Ops2.Max: operation = (X, Y) => Math.Max(X, Y); break;

                default: throw new ArgumentOutOfRangeException("Unsupported operation");
            }

            Data[] dataSet = new Data[datasetLength];

            for (int i = 0; i < datasetLength; i++)
            {
                double n1 = 0.0;
                double n2 = 0.0;
                double res = 0.0;

                while (res == 0)
                {
                    n1 = scale * GRandom.NextDouble();
                    n2 = scale * GRandom.NextDouble();
                    res = operation(n1, n2);
                }

                dataSet[i] = new Data(new double[] { n1, n2 }, new double[] { res }, normalizer);
            }

            return dataSet;
        }        
    }
}
