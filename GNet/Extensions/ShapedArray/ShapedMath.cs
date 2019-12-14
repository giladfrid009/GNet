using System;

namespace GNet.Extensions.ShapedArray.Math
{
    public static class ExtensionsShapedMath
    {
        public static TOut Accumulate<TSource, TOut>(this ShapedArray<TSource> source, TOut seed, Func<TOut, TSource, TOut> accumulator)
        {
            TOut res = seed;

            for (int i = 0; i < source.Length; i++)
            {
                res = accumulator(res, source[i]);
            }

            return res;
        }

        public static double Sum<TSource>(this ShapedArray<TSource> source, Func<TSource, double> selector)
        {
            double sum = default;

            for (int i = 0; i < source.Length; i++)
            {
                sum += selector(source[i]);
            }

            return sum;
        }

        public static double Avarage(this ShapedArray<double> source)
        {
            double sum = default;

            for (int i = 0; i < source.Length; i++)
            {
                sum += source[i];
            }

            return sum / source.Length;
        }

        public static double Min(this ShapedArray<double> source)
        {
            double min = source[0];

            for (int i = 1; i < source.Length; i++)
            {
                if (source[i] < min)
                {
                    min = source[i];
                }
            }

            return min;
        }

        public static double Max(this ShapedArray<double> source)
        {
            double max = source[0];

            for (int i = 1; i < source.Length; i++)
            {
                if (source[i] > max)
                {
                    max = source[i];
                }
            }

            return max;
        }
    }
}
