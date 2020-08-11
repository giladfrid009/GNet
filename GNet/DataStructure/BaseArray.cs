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

        public void ForEach(Action<T> action)
        {
            ForEach((X, i) => action(X));
        }     

        public double Min(Func<T, double> selector)
        {
            double minVal = selector(InternalArray[0]);

            for (int i = 1; i < Length; i++)
            {
                double val = selector(InternalArray[i]);

                if (val < minVal)
                {
                    minVal = val;
                }
            }

            return minVal;
        }

        public double Max(Func<T, double> selector)
        {
            double maxVal = selector(InternalArray[0]);

            for (int i = 1; i < Length; i++)
            {
                double val = selector(InternalArray[i]);

                if (val > maxVal)
                {
                    maxVal = val;
                }
            }

            return maxVal;
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