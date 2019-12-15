using System;

namespace GNet.Extensions.ShapedArray
{
    public static class ExtensionsShapedGeneric
    {
        public static ShapedArray<TOut> Select<TSource, TOut>(this ShapedReadOnlyArray<TSource> source, Func<TSource, TOut> selector)
        {
            ShapedArray<TOut> selected = new ShapedArray<TOut>(source.Shape);

            for (int i = 0; i < source.Length; i++)
            {
                selected[i] = selector(source[i]);
            }

            return selected;
        }

        public static ShapedArray<TSource> Combine<TSource>(this ShapedReadOnlyArray<TSource> source, ShapedReadOnlyArray<TSource> array, Func<TSource, TSource, TSource> selector)
        {
            if (source.Length != array.Length)
            {
                throw new ArgumentException("source and array length mismatch.");
            }

            ShapedArray<TSource> combined = new ShapedArray<TSource>(source.Shape);

            for (int i = 0; i < source.Length; i++)
            {
                combined[i] = selector(source[i], array[i]);
            }

            return combined;
        }
    }
}
