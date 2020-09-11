using System;

namespace GNet
{
    [Serializable]
    public class Array<T>
    {
        public static Array<T> Empty { get; } = new Array<T>(Array.Empty<T>(), true);

        public int Length { get; }
        public T this[int i] => internalArray[i];

        protected readonly T[] internalArray;

        protected Array(Array<T> array)
        {
            Length = array.Length;
            internalArray = array.internalArray;
        }

        protected Array(T[] array, bool asRef = false)
        {
            Length = array.Length;

            if (asRef)
            {
                internalArray = array;
            }
            else
            {
                internalArray = new T[Length];

                Array.Copy(array, 0, internalArray, 0, Length);
            }
        }

        public Array(params T[] elements) : this(elements, false)
        {
        }

        public Array(int length, Func<T> element)
        {
            Length = length;
            internalArray = new T[length];

            for (int i = 0; i < length; i++)
            {
                internalArray[i] = element();
            }
        }

        public static Array<T> FromRef(params T[] array)
        {
            return new Array<T>(array, true);
        }

        public ShapedArray<T> ToShape(Shape shape)
        {
            return ShapedArray<T>.FromRef(shape, internalArray);
        }

        public void ForEach(Action<T> action)
        {
            for (int i = 0; i < Length; i++)
            {
                action(internalArray[i]);
            }
        }

        public void ForEach(Action<T, int> action)
        {
            for (int i = 0; i < Length; i++)
            {
                action(internalArray[i], i);
            }
        }

        public Array<TRes> Select<TRes>(Func<T, TRes> selector)
        {
            var selected = new TRes[Length];

            for (int i = 0; i < Length; i++)
            {
                selected[i] = selector(internalArray[i]);
            }

            return Array<TRes>.FromRef(selected);
        }

        public Array<TRes> Select<TRes>(Func<T, int, TRes> selector)
        {
            var selected = new TRes[Length];

            for (int i = 0; i < Length; i++)
            {
                selected[i] = selector(internalArray[i], i);
            }

            return Array<TRes>.FromRef(selected);
        }              

        public double Min(Func<T, double> selector)
        {
            double min = double.MaxValue;

            for (int i = 0; i < Length; i++)
            {
                min = Math.Min(min, selector(internalArray[i]));
            }

            return min;
        }

        public double Max(Func<T, double> selector)
        {
            double max = double.MinValue;

            for (int i = 0; i < Length; i++)
            {
                max = Math.Max(max, selector(internalArray[i]));
            }

            return max;
        }

        public double Sum(Func<T, double> selector)
        {
            double sum = 0.0;

            for (int i = 0; i < Length; i++)
            {
                sum += selector(internalArray[i]);
            }

            return sum;
        }

        public double Sum(Array<T> other, Func<T, T, double> selector)
        {
            if (other.Length != Length)
            {
                throw new RankException(nameof(other));
            }

            double sum = 0.0;

            for (int i = 0; i < Length; i++)
            {
                sum += selector(internalArray[i], other[i]);
            }

            return sum;
        }

        public double Average(Func<T, double> selector)
        {
            return Sum(selector) / Length;
        }

        public double Average(Array<T> other, Func<T, T, double> selector)
        {
            return Sum(other, selector) / Length;
        }
    }
}