using System;
using System.Threading;
using System.Threading.Tasks;

namespace GNet.Extensions
{
    [Serializable]
    public class MultiCPU : IExtender
    {
        public ArrayImmutable<TOut> Select<TSource, TOut>(IArray<TSource> source, Func<TSource, int, TOut> selector)
        {
            var selected = new TOut[source.Length];

            Parallel.For(0, source.Length, i => selected[i] = selector(source[i], i));

            return new ArrayImmutable<TOut>(selected);
        }

        public ArrayImmutable<TSource> Combine<TSource>(IArray<TSource> source, IArray<TSource> array, Func<TSource, TSource, TSource> selector)
        {
            if (source.Length != array.Length)
            {
                throw new ArgumentException("source and array length mismatch.");
            }

            var combined = new TSource[source.Length];

            Parallel.For(0, source.Length, i => combined[i] = selector(source[i], array[i]));

            return new ArrayImmutable<TSource>(combined);
        }

        // todo: implement multithreading
        public double Accumulate<TSource>(IArray<TSource> source, double seed, Func<double, TSource, double> selector)
        {
            double res = seed;

            for (int i = 0; i < source.Length; i++)
            {
                res = selector(res, source[i]);
            }

            return res;
        }

        // todo: implement multithreading
        public double Sum<TSource>(IArray<TSource> source, Func<TSource, double> selector)
        {
            double sum = 0.0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += selector(source[i]);
            }

            return sum;
        }

        // todo: implement multithreading
        public double Min(IArray<double> source)
        {
            double min = source[0];

            for (int i = 1; i < source.Length; i++)
            {
                if (source[i] < min)
                {
                    min = source[i];
                }
            }

            return min;
        }

        // todo: implement multithreading
        public double Max(IArray<double> source)
        {
            double max = source[0];

            for (int i = 1; i < source.Length; i++)
            {
                if (source[i] > max)
                {
                    max = source[i];
                }
            }

            return max;
        }

        public void ForEach<TSource>(IArray<TSource> source, Action<TSource, int> action)
        {
            Parallel.For(0, source.Length, i => action(source[i], i));
        }

        public IExtender Clone()
        {
            return new MultiCPU();
        }
    }
}
