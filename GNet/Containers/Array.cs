using System;

namespace GNet
{
    [Serializable]
    public class Array<T>
    {
        public static Array<T> Empty { get; } = new Array<T>(Array.Empty<T>(), true);
        
        protected T[] InternalArray { get; }
        public int Length { get; }
        public T this[int i] => InternalArray[i];

        protected Array(Array<T> array)
        {
            Length = array.Length;
            InternalArray = array.InternalArray;
        }

        protected Array(T[] array, bool asRef = false)
        {
            Length = array.Length;

            if (asRef)
            {
                InternalArray = array;
            }
            else
            {
                InternalArray = new T[Length];

                Array.Copy(array, 0, InternalArray, 0, Length);
            }
        }

        public Array(params T[] elements) : this(elements, false)
        {
        }

        public Array(int length, Func<T> element)
        {
            Length = length;
            InternalArray = new T[length];

            for (int i = 0; i < length; i++)
            {
                InternalArray[i] = element();
            }
        }

        public static Array<T> FromRef(params T[] array)
        {
            return new Array<T>(array, true);
        }

        public ShapedArray<T> ToShape(Shape shape)
        {
            return ShapedArray<T>.FromRef(shape, InternalArray);
        }

        public void ForEach(Action<T> action)
        {
            ForEach((X, i) => action(X));
        }

        public void ForEach(Action<T, int> action)
        {
            for (int i = 0; i < Length; i++)
            {
                action(InternalArray[i], i);
            }
        }

        public Array<TRes> Select<TRes>(Func<T, TRes> selector)
        {
            return Select((X, i) => selector(X));
        }

        public Array<TRes> Select<TRes>(Func<T, int, TRes> selector)
        {
            var selected = new TRes[Length];

            for (int i = 0; i < Length; i++)
            {
                selected[i] = selector(InternalArray[i], i);
            }

            return Array<TRes>.FromRef(selected);
        }              

        public double Min(Func<T, double> selector)
        {
            double min = double.MaxValue;

            for (int i = 0; i < Length; i++)
            {
                min = Math.Min(min, selector(InternalArray[i]));
            }

            return min;
        }

        public double Max(Func<T, double> selector)
        {
            double max = double.MinValue;

            for (int i = 0; i < Length; i++)
            {
                max = Math.Max(max, selector(InternalArray[i]));
            }

            return max;
        }

        public double Sum(Func<T, double> selector)
        {
            double sum = 0.0;

            for (int i = 0; i < Length; i++)
            {
                sum += selector(InternalArray[i]);
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
                sum += selector(InternalArray[i], other[i]);
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