using System;

namespace GNet.Extensions.ShapedArray
{
    public static class ExtensionsShapedGeneric
    {
        public static ShapedArray<TOut> Select<TSource, TOut>(this ShapedArray<TSource> source, Func<TSource, TOut> selector)
        {
            TOut[] selected = new TOut[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                selected[i] = selector(source[i]);
            }

            return new ShapedArray<TOut>(source.Shape, selected);
        }

        // todo: use to multiply between a kernel and the input
        public static ShapedArray<TSource> Combine<TSource>(this ShapedArray<TSource> source, ShapedArray<TSource> array, Func<TSource, TSource, TSource> selector)
        {
            if (source.Length != array.Length)
            {
                throw new ArgumentException("source and array length mismatch.");
            }

            TSource[] combined = new TSource[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                combined[i] = selector(source[i], array[i]);
            }

            return new ShapedArray<TSource>(source.Shape, combined);
        }

        public static ShapedArray<double> Multiply(this ShapedArray<double> source, ShapedArray<double> array)
        {
            return source.Combine(array, (X, Y) => X * Y);
        }
    }
}
