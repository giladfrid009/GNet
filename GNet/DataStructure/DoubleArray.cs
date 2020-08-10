using System;
using System.Numerics;

namespace GNet
{
    [Serializable]
    public class DoubleArray : Array<double>
    {
        protected DoubleArray(double[] vals, bool asRef = false) : base(vals, asRef)
        {
            if (Length == 0)
            {
                throw new ArgumentException($"{nameof(vals)} must be at least of length 1.");
            }
        }

        public DoubleArray(params double[] vals) : this(vals, false)
        {
        }

        public DoubleArray(int length, double element) : base(new double[length], true)
        {
            if(length < 1)
            {
                throw new ArgumentException($"{nameof(length)} must be at least 1.");
            }

            for (int i = 0; i < Length; i++)
            {
                InternalArray[i] = element;
            }
        }

        public static new DoubleArray FromRef(params double[] array)
        {
            return new DoubleArray(array, true);
        }

        public double Min()
        {
            var vecMin = new Vector<double>(double.MaxValue);
            double min = InternalArray[0];
            int stride = Vector<double>.Count;
            int i;

            for (i = 1; i <= Length - stride; i += stride)
            {
                vecMin = Vector.Min(vecMin, new Vector<double>(InternalArray, i));
            }

            for (var j = 0; j < stride; ++j)
            {
                min = Math.Min(min, vecMin[j]);
            }

            for (; i < Length; ++i)
            {
                min = Math.Min(min, InternalArray[i]);
            }

            return min;
        }

        public double Max()
        {
            var vecMax = new Vector<double>(double.MaxValue);
            double max = InternalArray[0];
            int stride = Vector<double>.Count;
            int i;

            for (i = 1; i <= Length - stride; i += stride)
            {
                vecMax = Vector.Max(vecMax, new Vector<double>(InternalArray, i));
            }

            for (var j = 0; j < stride; ++j)
            {
                max = Math.Max(max, vecMax[j]);
            }

            for (; i < Length; ++i)
            {
                max = Math.Max(max, InternalArray[i]);
            }

            return max;
        }

        public double Sum()
        {
            var vecSum = Vector<double>.Zero;
            int stride = Vector<double>.Count;
            double sum;
            int i;

            for (i = 0; i <= Length - stride; i += stride)
            {
                vecSum += new Vector<double>(InternalArray, i);
            }

            sum = Vector.Dot(vecSum, Vector<double>.One);

            for (; i < Length; i++)
            {
                sum += InternalArray[i];
            }

            return sum;
        }

        public double Average()
        {
            return Sum() / Length;
        }
    }
}
