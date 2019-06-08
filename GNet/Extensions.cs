using System;
using System.Collections.Generic;
using GNet.GlobalRandom;

namespace GNet.Extensions
{
    public static class Extensions
    {
        public static TSource[] Flatten<TSource>(this Array source)
        {
            List<TSource> flattened = new List<TSource>();

            foreach (var element in source)
            {
                if (element is Array)
                {
                    flattened.AddRange(Flatten<TSource>(element as Array));
                }
                else if (element is TSource)
                {
                    flattened.Add((TSource)element);
                }
                else
                {
                    throw new ArrayTypeMismatchException("array element type mismatch");
                }
            }

            return flattened.ToArray();
        }

        public static TSource[] Shuffle<TSource>(this TSource[] source)
        {
            TSource[] shuffled = source.Select(X => X);

            for (int i = 0; i < shuffled.Length; i++)
            {
                int index = GRandom.Next(i, shuffled.Length);

                var temp = shuffled[i];
                shuffled[i] = shuffled[index];
                shuffled[index] = temp;
            }

            return shuffled;
        }

        public static TOut[] Select<TSource, TOut>(this TSource[] source, Func<TSource, TOut> selector)
        {
            TOut[] selected = new TOut[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                selected[i] = selector(source[i]);
            }

            return selected;
        }

        public static TSource[] Combine<TSource>(this TSource[] source, TSource[] array, Func<TSource, TSource, TSource> selector)
        {
            int minIndex = Math.Min(source.Length, array.Length);

            TSource[] combined = new TSource[minIndex];

            for (int i = 0; i < minIndex; i++)
            {
                combined[i] = selector(source[i], array[i]);
            }

            return combined;
        }

        public static TSource Accumulate<TSource>(this TSource[] source, TSource seed, Func<TSource, TSource, TSource> accumulator)
        {
            TSource res = seed;

            for (int i = 0; i < source.Length; i++)
            {
                res = accumulator(res, source[i]);
            }

            return res;
        }

        public static double Sum(this double[] source)
        {
            double sum = default;

            for (int i = 0; i < source.Length; i++)
            {
                sum += source[i];
            }

            return sum;
        }

        public static double Sum<TSource>(this TSource[] source, Func<TSource, double> summer)
        {
            double sum = default;

            for (int i = 0; i < source.Length; i++)
            {
                sum += summer(source[i]);
            }

            return sum;
        }          

        public static void ForEach<TSource>(this TSource[] source, Action<TSource> action)
        {
            for (int i = 0; i < source.Length; i++)
            {
                action(source[i]);
            }
        }

        public static void ForEach<TSource>(this TSource[] source, Action<TSource, int> action)
        {
            for (int i = 0; i < source.Length; i++)
            {
                action(source[i], i);
            }
        }
    }
}

