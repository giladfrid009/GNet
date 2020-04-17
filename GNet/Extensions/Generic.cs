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

        public static ImmutableArray<TSource> Combine<TSource>(this ImmutableArray<TSource> source, ImmutableArray<TSource> other, Func<TSource, TSource, TSource> selector)
        {
            int length = source.Length;

            if (length != other.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(other));
            }

            var combined = new TSource[length];

            for (int i = 0; i < length; i++)
            {
                combined[i] = selector(source[i], other[i]);
            }

            return ImmutableArray<TSource>.FromRef(combined);
        }

        public static ImmutableArray<TSource> Concat<TSource>(this ImmutableArray<TSource> source, ImmutableArray<TSource> other)
        {
            int length1 = source.Length;
            int length2 = other.Length;

            TSource[] concated = new TSource[length1 + length2];

            for (int i = 0; i < length1; i++)
            {
                concated[i] = source[i];
            }

            for (int i = 0; i < length2; i++)
            {
                concated[i + length1] = other[i];
            }

            return ImmutableArray<TSource>.FromRef(concated);
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