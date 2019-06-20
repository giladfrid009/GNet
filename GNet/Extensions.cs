using GNet.GlobalRandom;
using System;
using System.Collections.Generic;

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

        public static void Shuffle<TSource>(this TSource[] source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                int index = GRandom.Next(i, source.Length);

                var temp = source[i];
                source[i] = source[index];
                source[index] = temp;
            }
        }

        // ------- math
        // todo: maybe remove it as extensions and implement it as a Math in GNet.Math? cuz after all it extends only for math ops and not for all arrays.

        public static TOut Accumulate<TSource, TOut>(this TSource[] source, TOut seed, Func<TOut, TSource, TOut> accumulator)
        {
            TOut res = seed;

            source.ForEach(X => res = accumulator(res, X));

            return res;
        }

        public static double Sum(this double[] source)
        {
            double sum = default;

            source.ForEach(X => sum += X);

            return sum;
        }

        public static double Mean(this double[] source)
        {
            return source.Sum() / source.Length;
        }

        public static double Min(this double[] source)
        {
            double min = source[0];

            source.ForEach(X =>
            {
                if (X < min)
                    min = X;
            });

            return min;
        }

        public static double Max(this double[] source)
        {
            double max = source[0];

            source.ForEach(X =>
            {
                if (X > max)
                    max = X;
            });

            return max;
        }

        // ------- misc

        public static void Print<TSource>(this TSource obj) where TSource : struct
        {
            Console.WriteLine(obj);
        }

        public static void Print<TSource>(this TSource[] objs) where TSource : struct
        {
            objs.ForEach(X => Console.WriteLine(X));
        }
    }
}

