using System;

namespace GNet
{
    public static class MathExtensions
    {
        public static double Accumulate<TSource>(this IArray<TSource> source, double seed, Func<double, TSource, double> selector)
        {
            double res = seed;

            for (int i = 0; i < source.Length; i++)
            {
                res = selector(res, source[i]);
            }

            return res;
        }

        public static double Sum<TSource>(this IArray<TSource> source, Func<TSource, double> selector)
        {
            double sum = 0.0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += selector(source[i]);
            }

            return sum;
        }

        public static double Sum(this IArray<double> source)
        {
            double sum = 0.0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += source[i];
            }

            return sum;
        }

        public static double Min(this IArray<double> source)
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

        public static double Max(this IArray<double> source)
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

        public static double Avarage(this IArray<double> source)
        {
            return Sum(source) / source.Length;
        }
    }
}
