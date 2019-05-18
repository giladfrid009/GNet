using System;
using GNet.GlobalRandom;

namespace GNet.Datasets
{
    internal class MathOperations
    {
        public enum Ops { Add, Sub, Mul, Div, Pow, Root }

        public static Data[] GenerateDataset(Ops mathOperation, double valueScale, int datasetLength)
        {
            Func<double, double, double> operation;

            switch (mathOperation)
            {
                case Ops.Add: operation = (X, Y) => X + Y; break;

                case Ops.Sub: operation = (X, Y) => X - Y; break;

                case Ops.Mul: operation = (X, Y) => X * Y; break;

                case Ops.Div: operation = (X, Y) => X / Y; break;

                case Ops.Pow: operation = (X, Y) => Math.Pow(X, Y); break;

                case Ops.Root: operation = (X, Y) => Math.Pow(X, 1.0 / Y); break;

                default: throw new ArgumentOutOfRangeException("Unsupported mathOperation");
            }

            Data[] dataSet = new Data[datasetLength];

            for (int i = 0; i < datasetLength; i++)
            {
                double n1 = 0.0;
                double n2 = 0.0;
                double res = 0.0;

                while (res == 0)
                {
                    n1 = valueScale * GRandom.NextDouble();
                    n2 = valueScale * GRandom.NextDouble();
                    res = operation(n1, n2);
                }

                dataSet[i] = new Data(new double[] { n1, n2 }, new double[] { res });
            }

            return dataSet;
        }
    }
}
