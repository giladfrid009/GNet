using System;

namespace GNet
{
    public static class MathExtensions
    {
        public static double Avarage<TSource, TOther>(this IArray<TSource> source, IArray<TOther> other, Func<TSource, TOther, double> selector)
        {
            return Sum(source, other, selector) / source.Length;
        }

        public static double Avarage<TSource>(this IArray<TSource> source, Func<TSource, double> selector)
        {
            return Sum(source, selector) / source.Length;
        }       

        public static double Avarage(this IArray<double> source)
        {
            return Sum(source) / source.Length;
        }

        public static double Max<TSource>(this IArray<TSource> source, Func<TSource, double> selector)
        {
            int length = source.Length;
            double max = selector(source[0]);

            for (int i = 1; i < length; i++)
            {
                double val = selector(source[i]);

                if (val > max)
                {
                    max = val;
                }
            }

            return max;
        }

        public static double Max(this IArray<double> source)
        {
            int length = source.Length;
            double max = source[0];

            for (int i = 1; i < length; i++)
            {
                if (source[i] > max)
                {
                    max = source[i];
                }
            }

            return max;
        }

        public static double Min<TSource>(this IArray<TSource> source, Func<TSource, double> selector)
        {
            int length = source.Length;
            double min = selector(source[0]);

            for (int i = 1; i < length; i++)
            {
                double val = selector(source[i]);

                if (val < min)
                {
                    min = val;
                }
            }

            return min;
        }

        public static double Min(this IArray<double> source)
        {
            int length = source.Length;
            double min = source[0];

            for (int i = 1; i < length; i++)
            {
                if (source[i] < min)
                {
                    min = source[i];
                }
            }

            return min;
        }

        public static double Sum<TSource, TOther>(this IArray<TSource> source, IArray<TOther> other, Func<TSource, TOther, double> selector)
        {
            int length = source.Length;

            if(other.Length != length)
            {
                throw new ArgumentOutOfRangeException(nameof(other));
            }

            double sum = 0.0;

            for (int i = 0; i < length; i++)
            {
                sum += selector(source[i], other[i]);
            }

            return sum;
        }

        public static double Sum<TSource>(this IArray<TSource> source, Func<TSource, double> selector)
        {
            int length = source.Length;
            double sum = 0.0;

            for (int i = 0; i < length; i++)
            {
                sum += selector(source[i]);
            }

            return sum;
        }

        public static double Sum(this IArray<double> source)
        {
            int length = source.Length;
            double sum = 0.0;

            for (int i = 0; i < length; i++)
            {
                sum += source[i];
            }

            return sum;
        }
    }
}