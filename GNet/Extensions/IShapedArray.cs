using System;

namespace GNet.Extensions.IShapedArray
{
    public static class IShapedArrayExtensions
    {
        public static ShapedArrayImmutable<TOut> Select<TSource, TOut>(this IShapedArray<TSource> source, Func<TSource, TOut> selector)
        {
            var selected = new TOut[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                selected[i] = selector(source[i]);
            }

            return new ShapedArrayImmutable<TOut>(source.Shape, selected);
        }

        public static ShapedArrayImmutable<TSource> Combine<TSource>(this IShapedArray<TSource> source, IShapedArray<TSource> array, Func<TSource, TSource, TSource> selector)
        {
            if (source.Length != array.Length)
            {
                throw new ArgumentException("source and array length mismatch.");
            }

            var combined = new TSource[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                combined[i] = selector(source[i], array[i]);
            }

            return new ShapedArrayImmutable<TSource>(source.Shape, combined);
        }

        public static ShapedArrayImmutable<double> Multiply(this IShapedArray<double> source, IShapedArray<double> array)
        {
            return source.Combine(array, (X, Y) => X * Y);
        }
    }
}
