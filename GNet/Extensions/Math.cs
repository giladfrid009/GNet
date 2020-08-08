using System;

namespace GNet
{
    public static class MathExtensions
    {
        public static double Average<T, TOther>(this in ImmutableArray<T> source, in ImmutableArray<TOther> other, Func<T, TOther, double> selector)
        {
            return Sum(source, other, selector) / source.Length;
        }

        public static double Average<T>(this in ImmutableArray<T> source, Func<T, double> selector)
        {
            return Sum(source, selector) / source.Length;
        }

        public static double Average(this in ImmutableArray<double> source)
        {
            return Sum(source) / source.Length;
        }

        public static double Max<T>(this in ImmutableArray<T> source, Func<T, double> selector)
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

        public static double Max(this in ImmutableArray<double> source)
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

        public static double Min<T>(this in ImmutableArray<T> source, Func<T, double> selector)
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

        public static double Min(this in ImmutableArray<double> source)
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

        public static double Sum<T, TOther>(this in ImmutableArray<T> source, in ImmutableArray<TOther> other, Func<T, TOther, double> selector)
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

        public static double Sum<T>(this in ImmutableArray<T> source, Func<T, double> selector)
        {
            double sum = 0.0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += selector(source[i]);
            }

            return sum;
        }

        public static double Sum(this in ImmutableArray<double> source)
        {
            double sum = 0.0;

            for (int i = 0; i < source.Length; i++)
            {
                sum += source[i];
            }

            return sum;
        }

        public static double Average<T, TOther>(this in ImmutableShapedArray<T> source, in ImmutableArray<TOther> other, Func<T, TOther, double> selector)
        {
            return Sum((ImmutableArray<T>)source, other, selector) / source.Length;
        }

        public static double Average<T>(this in ImmutableShapedArray<T> source, Func<T, double> selector)
        {
            return Sum((ImmutableArray<T>)source, selector) / source.Length;
        }

        public static double Average(this in ImmutableShapedArray<double> source)
        {
            return Sum((ImmutableArray<double>)source) / source.Length;
        }

        public static double Max<T>(this in ImmutableShapedArray<T> source, Func<T, double> selector)
        {
            return Max((ImmutableArray<T>)source, selector);
        }

        public static double Max(this in ImmutableShapedArray<double> source)
        {
            return Max((ImmutableArray<double>)source);
        }

        public static double Min<T>(this in ImmutableShapedArray<T> source, Func<T, double> selector)
        {
            return Min((ImmutableArray<T>)source, selector);
        }

        public static double Min(this in ImmutableShapedArray<double> source)
        {
            return Min((ImmutableArray<double>)source);
        }

        public static double Sum<T, TOther>(this in ImmutableShapedArray<T> source, in ImmutableArray<TOther> other, Func<T, TOther, double> selector)
        {
            return Sum((ImmutableArray<T>)source, other, selector);
        }

        public static double Sum<T>(this in ImmutableShapedArray<T> source, Func<T, double> selector)
        {
            return Sum((ImmutableArray<T>)source, selector);
        }

        public static double Sum(this in ImmutableShapedArray<double> source)
        {
            return Sum((ImmutableArray<double>)source);
        }
    }
}