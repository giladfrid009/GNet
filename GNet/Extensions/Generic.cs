using System;

namespace GNet
{
    public static class GenericExtensions
    {
        public static ImmutableArray<TRes> Select<T, TRes>(this in ImmutableArray<T> source, Func<T, int, TRes> selector)
        {
            var selected = new TRes[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                selected[i] = selector(source[i], i);
            }

            return ImmutableArray<TRes>.FromRef(selected);
        }       

        public static ImmutableArray<TRes> Select<T, TRes>(this in ImmutableArray<T> source, Func<T, TRes> selector)
        {
            return Select(source, (X, i) => selector(X));
        }

        public static void ForEach<T>(this in ImmutableArray<T> source, Action<T, int> action)
        {
            for (int i = 0; i < source.Length; i++)
            {
                action(source[i], i);
            }
        }

        public static void ForEach<T>(this in ImmutableArray<T> source, Action<T> action)
        {
            ForEach(source, (X, i) => action(X));
        }

        public static ImmutableShapedArray<TRes> Select<T, TRes>(this in ImmutableShapedArray<T> source, Func<T, int, TRes> selector)
        {
            return Select((ImmutableArray<T>)source, selector).ToShape(source.Shape);
        }

        public static ImmutableShapedArray<TRes> Select<T, TRes>(this in ImmutableShapedArray<T> source, Func<T, TRes> selector)
        {
            return Select((ImmutableArray<T>)source, (X, i) => selector(X)).ToShape(source.Shape);
        }

        public static void ForEach<T>(this in ImmutableShapedArray<T> source, Action<T, int> action)
        {
            ForEach((ImmutableArray<T>)source, action);
        }

        public static void ForEach<T>(this in ImmutableShapedArray<T> source, Action<T> action)
        {
            ForEach((ImmutableArray<T>)source, (X, i) => action(X));
        }
    }
}