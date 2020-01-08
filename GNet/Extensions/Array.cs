using System;

namespace GNet.Extensions.Array
{
    public static class ArrayExtensions
    {
        public static ArrayImmutable<TSource> ToImmutable<TSource>(this TSource[] source)
        {
            return new ArrayImmutable<TSource>(source);
        }

        public static TOut[] Select<TSource, TOut>(this TSource[] source, Func<TSource, TOut> selector)
        {
            var selected = new TOut[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                selected[i] = selector(source[i]);
            }

            return selected;
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
            source.ForEach((X, _) => action(X));
        }
    }
}

