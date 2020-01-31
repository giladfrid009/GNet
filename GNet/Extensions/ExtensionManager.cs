using System;

namespace GNet
{
    [Serializable]
    public static class ExtensionManager
    {
        public static IExtender Extender { get; private set; }

        static ExtensionManager()
        {
            Extender = new Extensions.CPU();
        }

        public static void Initialize(IExtender extender)
        {
            Extender = extender.Clone();
        }

        public static ArrayImmutable<TOut> Select<TSource, TOut>(this IArray<TSource> source, Func<TSource, int, TOut> selector)
        {
            return Extender.Select(source, selector);
        }

        public static ArrayImmutable<TSource> Combine<TSource>(this IArray<TSource> source, IArray<TSource> array, Func<TSource, TSource, TSource> selector)
        {
            return Extender.Combine(source, array, selector);
        }

        public static TOut Accumulate<TSource, TOut>(this IArray<TSource> source, TOut seed, Func<TOut, TSource, TOut> accumulator)
        {
            return Extender.Accumulate(source, seed, accumulator);
        }

        public static double Sum<TSource>(this IArray<TSource> source, Func<TSource, double> selector)
        {
            return Extender.Sum(source, selector);
        }

        public static double Avarage(this IArray<double> source)
        {
            return Extender.Avarage(source);
        }

        public static double Min(this IArray<double> source)
        {
            return Extender.Min(source);
        }

        public static double Max(this IArray<double> source)
        {
            return Extender.Max(source);
        }

        public static void ForEach<TSource>(this IArray<TSource> source, Action<TSource, int> action)
        {
            Extender.ForEach(source, action);
        }

        public static ArrayImmutable<TOut> Select<TSource, TOut>(this IArray<TSource> source, Func<TSource, TOut> selector)
        {
            return Select(source, (X, i) => selector(X));
        }

        public static ShapedArrayImmutable<TOut> Select<TSource, TOut>(this ShapedArrayImmutable<TSource> source, Func<TSource, TOut> selector)
        {
            return Select((IArray<TSource>)source, selector).ToShape(source.Shape);
        }

        public static ShapedArrayImmutable<TSource> Combine<TSource>(this ShapedArrayImmutable<TSource> source, IArray<TSource> array, Func<TSource, TSource, TSource> selector)
        {
            return Combine((IArray<TSource>)source, array, selector).ToShape(source.Shape);
        }

        public static void ForEach<TSource>(this IArray<TSource> source, Action<TSource> action)
        {
            ForEach(source, (X, i) => action(X));
        }
    }
}
