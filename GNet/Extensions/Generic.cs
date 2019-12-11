using GNet.GlobalRandom;
using System;

namespace GNet.Extensions.Generic
{
    public static class ExtensionsGeneric
    {
        public static TOut[] Select<TSource, TOut>(this TSource[] source, Func<TSource, int, TOut> selector)
        {
            TOut[] selected = new TOut[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                selected[i] = selector(source[i], i);
            }

            return selected;
        }

        public static TOut[] Select<TSource, TOut>(this TSource[] source, Func<TSource, TOut> selector)
        {
            return source.Select((X, i) => selector(X));
        }

        public static TSource[] Combine<TSource>(this TSource[] source, TSource[] array, Func<TSource, TSource, TSource> selector)
        {
            int minIndex = System.Math.Min(source.Length, array.Length);

            TSource[] combined = new TSource[minIndex];

            for (int i = 0; i < minIndex; i++)
            {
                combined[i] = selector(source[i], array[i]);
            }

            return combined;
        }

        public static void ForEach<TSource>(this TSource[] source, Action<TSource, int> action)
        {
            for (int i = 0; i < source.Length; i++)
            {
                action(source[i], i);
            }
        }

        public static void ForEach<TSource>(this TSource[] source, Action<TSource> action)
        {
            source.ForEach((X, i) => action(X));
        }

        public static void Shuffle<TSource>(this TSource[] source)
        {
            for (int i = 0; i < source.Length; i++)
            {
                int index = GRandom.Next(i, source.Length);

                TSource temp = source[i];
                source[i] = source[index];
                source[index] = temp;
            }
        }
    }
}

