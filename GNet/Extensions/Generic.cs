using GNet.GlobalRandom;
using System;
using System.Collections.Generic;

namespace GNet.Extensions.Generic
{
    public static class ExtensionsGeneric
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
            int minIndex = System.Math.Min(source.Length, array.Length);

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
    }
}

