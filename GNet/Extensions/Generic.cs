using System;

namespace GNet
{
    public static class GenericExtensions
    {
        public static ImmutableArray<TResult> Select<TSource, TResult>(this ImmutableArray<TSource> source, Func<TSource, int, TResult> selector)
        {
            int length = source.Length;
            var selected = new TResult[length];

            for (int i = 0; i < length; i++)
            {
                selected[i] = selector(source[i], i);
            }

            return ImmutableArray<TResult>.FromRef(selected);
        }

        public static ImmutableArray<TResult> Select<TSource, TResult>(this ImmutableArray<TSource> source, Func<TSource, TResult> selector)
        {
            return Select(source, (X, i) => selector(X));
        }

        public static void ForEach<TSource>(this IArray<TSource> source, Action<TSource, int> action)
        {
            int length = source.Length;

            for (int i = 0; i < length; i++)
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