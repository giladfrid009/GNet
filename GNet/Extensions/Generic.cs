using System;

namespace GNet
{
    public static class GenericExtensions
    {
        public static ImmutableArray<TRes> Select<T, TRes>(this ImmutableArray<T> source, Func<T, int, TRes> selector)
        {
            int length = source.Length;
            var selected = new TRes[length];

            for (int i = 0; i < length; i++)
            {
                selected[i] = selector(source[i], i);
            }

            return ImmutableArray<TRes>.FromRef(selected);
        }

        public static ImmutableArray<TRes> Select<T, TRes>(this ImmutableArray<T> source, Func<T, TRes> selector)
        {
            return Select(source, (X, i) => selector(X));
        }

        public static void ForEach<T>(this IArray<T> source, Action<T, int> action)
        {
            int length = source.Length;

            for (int i = 0; i < length; i++)
            {
                action(source[i], i);
            }
        }

        public static void ForEach<T>(this IArray<T> source, Action<T> action)
        {
            ForEach(source, (X, i) => action(X));
        }
    }
}