using GNet.GlobalRandom;
using System;

namespace GNet.Extensions.ShapedArray.Generic
{
    public static class ExtensionsShapedGeneric
    {
        public static ShapedArray<TOut> Select<TSource, TOut>(this ShapedArray<TSource> source, Func<TSource, int, TOut> selector)
        {
            ShapedArray<TOut> selected = new ShapedArray<TOut>(source.Shape);

            for (int i = 0; i < source.Length; i++)
            {
                selected[i] = selector(source[i], i);
            }

            return selected;
        }

        public static ShapedArray<TOut> Select<TSource, TOut>(this ShapedArray<TSource> source, Func<TSource, TOut> selector)
        {
            return source.Select((X, i) => selector(X));
        }

        public static ShapedArray<TSource> Combine<TSource>(this ShapedArray<TSource> source, ShapedArray<TSource> array, Func<TSource, TSource, TSource> selector)
        {
            if (source.Shape.Equals(array.Shape) == false)
            {
                throw new ArgumentException("source and array shape mismatch.");
            }

            ShapedArray<TSource> combined = new ShapedArray<TSource>(source.Shape);

            for (int i = 0; i < source.Length; i++)
            {
                combined[i] = selector(source[i], array[i]);
            }

            return combined;
        }

        public static void ForEach<TSource>(this ShapedArray<TSource> source, Action<TSource, int> action)
        {
            for (int i = 0; i < source.Length; i++)
            {
                action(source[i], i);
            }
        }

        public static void ForEach<TSource>(this ShapedArray<TSource> source, Action<TSource> action)
        {
            source.ForEach((X, i) => action(X));
        }

        public static void Shuffle<TSource>(this ShapedArray<TSource> source)
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
