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
            int length = source.Length;
            double maxVal = selector(source[0]);

            for (int i = 1; i < length; i++)
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
            int length = source.Length;
            double maxVal = source[0];

            for (int i = 1; i < length; i++)
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
            int length = source.Length;
            double minVal = selector(source[0]);

            for (int i = 1; i < length; i++)
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
            int length = source.Length;
            double minVal = source[0];

            for (int i = 1; i < length; i++)
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
            int length = source.Length;

            if (other.Length != length)
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

        public static double Sum<T>(this IArray<T> source, Func<T, double> selector)
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