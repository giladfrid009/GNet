using NCollections;
using System;

namespace GNet
{
    public static class Extensions
    {
        public static void ForEach<T>(this Array<T> array, Action<T> action)
        {
            for (int i = 0; i < array.Length; i++)
            {
                action(array[i]);
            }
        }

        public static void ForEach<T>(this Array<T> array, Action<T, int> action)
        {
            for (int i = 0; i < array.Length; i++)
            {
                action(array[i], i);
            }
        }

        public static Array<U> Select<T, U>(this Array<T> array, Func<T, U> selector)
        {
            var selected = new U[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                selected[i] = selector(array[i]);
            }

            return Array<U>.FromRef(selected);
        }

        public static Array<U> Select<T, U>(this Array<T> array, Func<T, int, U> selector)
        {
            var selected = new U[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                selected[i] = selector(array[i], i);
            }

            return Array<U>.FromRef(selected);
        }

        public static double Min<T>(this Array<T> array, Func<T, double> selector)
        {
            double min = double.MaxValue;

            for (int i = 0; i < array.Length; i++)
            {
                min = Math.Min(min, selector(array[i]));
            }

            return min;
        }

        public static double Max<T>(this Array<T> array, Func<T, double> selector)
        {
            double max = double.MinValue;

            for (int i = 0; i < array.Length; i++)
            {
                max = Math.Max(max, selector(array[i]));
            }

            return max;
        }

        public static double Sum<T>(this Array<T> array, Func<T, double> selector)
        {
            double sum = 0.0;

            for (int i = 0; i < array.Length; i++)
            {
                sum += selector(array[i]);
            }

            return sum;
        }

        public static double Sum<T>(this Array<T> array, Array<T> other, Func<T, T, double> selector)
        {
            if (other.Length != array.Length)
            {
                throw new RankException(nameof(other));
            }

            double sum = 0.0;

            for (int i = 0; i < array.Length; i++)
            {
                sum += selector(array[i], other[i]);
            }

            return sum;
        }

        public static double Average<T>(this Array<T> array, Func<T, double> selector)
        {
            return Sum(array, selector) / array.Length;
        }

        public static double Average<T>(this Array<T> array, Array<T> other, Func<T, T, double> selector)
        {
            return Sum(array, other, selector) / array.Length;

        }

        public static ShapedArray<T> Reshape<T>(this Array<T> array, Shape shape)
        {
            return ShapedArray<T>.FromRef(shape, array.ToArray());
        }
    }
}
