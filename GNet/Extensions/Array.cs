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
            TOut[] selected = new TOut[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                selected[i] = selector(source[i]);
            }

            return selected;
        }

        public static TOut Accumulate<TSource, TOut>(this TSource[] source, TOut seed, Func<TOut, TSource, TOut> accumulator)
        {
            TOut res = seed;

            for (int i = 0; i < source.Length; i++)
            {
                res = accumulator(res, source[i]);
            }

            return res;
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
    }
}

