using System;

namespace GNet
{
    [Serializable]
    public abstract class BaseArray<T>
    {
        protected BaseArray(int length)
        {
            Length = length;
        }

        public int Length { get; }
        public T this[int i] { get => InternalArray[i]; }

        protected abstract T[] InternalArray { get; }

        public void ForEach(Action<T, int> action)
        {
            for (int i = 0; i < Length; i++)
            {
                action(InternalArray[i], i);
            }
        }

        public ShapedArray<T> ToShape(Shape shape)
        {
            return ShapedArray<T>.FromRef(shape, InternalArray);
        }

        public void ForEach(Action<T> action)
        {
            ForEach((X, i) => action(X));
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

        public double Sum(BaseArray<T> other, Func<T, T, double> selector)
        {
            if (other.Length != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(other));
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

        public double Average(BaseArray<T> other, Func<T, T, double> selector)
        {
            return Sum(other, selector) / Length;
        }
    }
}