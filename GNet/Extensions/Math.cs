using System;

namespace GNet
{
    public static class MathExtensions
    {
        public static double Average<T, TOther>(this IArray<T> source, IArray<TOther> other, Func<T, TOther, double> selector)
        {
            return Sum(source, other, selector) / source.Length;
        }

        public static double Average<T>(this IArray<T> source, Func<T, double> selector)
        {
            return Sum(source, selector) / source.Length;
        }

        public static double Average(this IArray<double> source)
        {
            return Sum(source) / source.Length;
        }

        public static double Max<T>(this IArray<T> source, Func<T, double> selector)
        {
            double maxVal = selector(source[0]);

            for (int i = 1; i < source.Length; i++)
            {
                double val = selector(source[i]);

                if (val > maxVal)
                {
                    maxVal = val;
                }
            }

            return maxVal;
        }

        public static double Max(this IArray<double> source)
        {
            double maxVal = source[0];

            for (int i = 1; i < source.Length; i++)
            {
                if (source[i] > maxVal)
                {
                    maxVal = source[i];
                }
            }

            return maxVal;
        }

        public static double Min<T>(this IArray<T> source, Func<T, double> selector)
        {
            double minVal = selector(source[0]);

            for (int i = 1; i < source.Length; i++)
            {
                double val = selector(source[i]);

                if (val < minVal)
                {
                    minVal = val;
                }
            }

            return minVal;
        }

        public static double Min(this IArray<double> source)
        {
            double minVal = source[0];

            for (int i = 1; i < source.Length; i++)
            {
                if (source[i] < minVal)
                {
                    minVal = source[i];
                }
            }

            return minVal;
        }

        public static double Sum<T, TOther>(this IArray<T> source, IArray<TOther> other, Func<T, TOther, double> selector)
        {
            if (other.Length != source.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(other));
            }

            double sum = 0.0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += selector(source[i], other[i]);
            }

            return sum;
        }

        public static double Sum<T>(this IArray<T> source, Func<T, double> selector)
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
    }
}