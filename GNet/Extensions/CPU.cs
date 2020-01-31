using System;

namespace GNet.Extensions
{
    [Serializable]
    public class CPU : IExtender
    {
        public ArrayImmutable<TOut> Select<TSource, TOut>(IArray<TSource> source, Func<TSource, int, TOut> selector)
        {
            var selected = new TOut[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                selected[i] = selector(source[i], i);
            }

            return new ArrayImmutable<TOut>(selected);
        }

        public ArrayImmutable<TSource> Combine<TSource>(IArray<TSource> source, IArray<TSource> array, Func<TSource, TSource, TSource> selector)
        {
            if (source.Length != array.Length)
            {
                throw new ArgumentException("source and array length mismatch.");
            }

            var combined = new TSource[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                combined[i] = selector(source[i], array[i]);
            }

            return new ArrayImmutable<TSource>(combined);
        }

        public TOut Accumulate<TSource, TOut>(IArray<TSource> source, TOut seed, Func<TOut, TSource, TOut> accumulator)
        {
            TOut res = seed;

            for (int i = 0; i < source.Length; i++)
            {
                res = accumulator(res, source[i]);
            }

            return res;
        }

        public double Sum<TSource>(IArray<TSource> source, Func<TSource, double> selector)
        {
            double sum = default;

            for (int i = 0; i < source.Length; i++)
            {
                sum += selector(source[i]);
            }

            return sum;
        }

        public double Avarage(IArray<double> source)
        {
            double sum = default;

            for (int i = 0; i < source.Length; i++)
            {
                sum += source[i];
            }

            return sum / source.Length;
        }

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
            for (int i = 0; i < source.Length; i++)
            {
                action(source[i], i);
            }
        }

        public IExtender Clone()
        {
            return new CPU();
        }
    }
}
