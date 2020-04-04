using System;
using System.Collections.Generic;

namespace GNet
{
    public static class GenericExtensions
    {
        public static ArrayImmutable<TOut> Select<TSource, TOut>(this IArray<TSource> source, Func<TSource, int, TOut> selector)
        {
            var selected = new TOut[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                selected[i] = selector(source[i], i);
            }

            return new ArrayImmutable<TOut>(in selected);
        }

        public static ArrayImmutable<TOut> Select<TSource, TOut>(this IArray<TSource> source, Func<TSource, TOut> selector)
        {
            return Select(source, (X, i) => selector(X));
        }

        public static ShapedArrayImmutable<TOut> Select<TSource, TOut>(this ShapedArrayImmutable<TSource> source, Func<TSource, TOut> selector)
        {
            return Select((IArray<TSource>)source, selector).ToShape(source.Shape);
        }

        public static ShapedArrayImmutable<TOut> Select<TSource, TOut>(this ShapedArrayImmutable<TSource> source, Func<TSource, int, TOut> selector)
        {
            return Select((IArray<TSource>)source, selector).ToShape(source.Shape);
        }

        public static ArrayImmutable<TSource> Combine<TSource>(this IArray<TSource> source, IArray<TSource> other, Func<TSource, TSource, TSource> selector)
        {
            if (source.Length != other.Length)
            {
                throw new ArgumentException("source and array length mismatch.");
            }

            var combined = new TSource[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                combined[i] = selector(source[i], other[i]);
            }

            return new ArrayImmutable<TSource>(in combined);
        }

        public static ShapedArrayImmutable<TSource> Combine<TSource>(this ShapedArrayImmutable<TSource> source, IArray<TSource> other, Func<TSource, TSource, TSource> selector)
        {
            return Combine((IArray<TSource>)source, other, selector).ToShape(source.Shape);
        }

        public static ArrayImmutable<TSource> Concat<TSource>(this IArray<TSource> source, IArray<TSource> other)
        {
            TSource[] concated = new TSource[source.Length + other.Length];

            for (int i = 0; i < source.Length; i++)
            {
                concated[i] = source[i];
            }

            for (int i = 0; i < other.Length; i++)
            {
                concated[i + source.Length] = other[i];
            }

            return new ArrayImmutable<TSource>(in concated);
        }

        public static void ForEach<TSource>(this IArray<TSource> source, Action<TSource, int> action)
        {
            for (int i = 0; i < source.Length; i++)
            {
                action(source[i], i);
            }
        }

        public static void ForEach<TSource>(this IArray<TSource> source, Action<TSource> action)
        {
            ForEach(source, (X, i) => action(X));
        }
    }
}