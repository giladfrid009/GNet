using System;
using System.Numerics;

namespace GNet
{
    [Serializable]
    public class VArray : Array<double>
    {
        static readonly int VStride = Vector<double>.Count;

        protected VArray(double[] vals, bool asRef = false) : base(vals, asRef)
        {            
        }

        public VArray(params double[] vals) : this(vals, false)
        {
        }

        public VArray(int length, Func<double> element) : base(length, element)
        {           
        }

        public static new VArray FromRef(params double[] array)
        {
            return new VArray(array, true);
        }

        public VArray Select(Func<Vector<double>, Vector<double>> vSelector, Func<double, double> selector)
        {
            return Select((X, i) => vSelector(X), (X, i) => selector(X));
        }

        public VArray Select(Func<Vector<double>, int, Vector<double>> vSelector, Func<double, int, double> selector)
        {
            var selected = new double[Length];
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                var vCur = vSelector(new Vector<double>(InternalArray, i), i);
                vCur.CopyTo(selected, i);
            }

            for (; i < Length; ++i)
            {
                selected[i] = selector(InternalArray[i], i);
            }

            return FromRef(selected);
        }

        public double Min()
        {
            var vMin = new Vector<double>(double.MaxValue);
            double min = double.MaxValue;
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                var vCur = new Vector<double>(InternalArray, i);
                vMin = Vector.Min(vMin, vCur);
            }

            for (var j = 0; j < VStride; ++j)
            {
                min = Math.Min(min, vMin[j]);
            }

            for (; i < Length; ++i)
            {
                min = Math.Min(min, InternalArray[i]);
            }

            return min;
        }

        public double Min(Func<Vector<double>, Vector<double>> vSelector, Func<double, double> selector)
        {
            var vMin = new Vector<double>(double.MaxValue);
            double min = double.MaxValue;
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                var vCur = vSelector(new Vector<double>(InternalArray, i));
                vMin = Vector.Min(vMin, vCur);
            }

            for (var j = 0; j < VStride; ++j)
            {
                min = Math.Min(min, vMin[j]);
            }

            for (; i < Length; ++i)
            {
                min = Math.Min(min, selector(InternalArray[i]));
            }

            return min;
        }

        public double Max()
        {
            var vMax = new Vector<double>(double.MinValue);
            double max = double.MinValue;
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                var vec = new Vector<double>(InternalArray, i);
                vMax = Vector.Max(vMax, vec);
            }

            for (var j = 0; j < VStride; ++j)
            {
                max = Math.Max(max, vMax[j]);
            }

            for (; i < Length; ++i)
            {
                max = Math.Max(max, InternalArray[i]);
            }

            return max;
        }

        public double Max(Func<Vector<double>, Vector<double>> vSelector, Func<double, double> selector)
        {
            var vMax = new Vector<double>(double.MinValue);
            double max = double.MinValue;
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                var vCur = vSelector(new Vector<double>(InternalArray, i));
                vMax = Vector.Max(vMax, vCur);
            }

            for (var j = 0; j < VStride; ++j)
            {
                max = Math.Max(max, vMax[j]);
            }

            for (; i < Length; ++i)
            {
                max = Math.Max(max, selector(InternalArray[i]));
            }

            return max;
        }

        public double Sum()
        {
            var vSum = Vector<double>.Zero;
            double sum;
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                vSum += new Vector<double>(InternalArray, i);
            }

            sum = Vector.Dot(vSum, Vector<double>.One);

            for (; i < Length; i++)
            {
                sum += InternalArray[i];
            }

            return sum;
        }     

        public double Sum(Func<Vector<double>, Vector<double>> vSelector, Func<double, double> selector)
        {
            var vSum = Vector<double>.Zero;
            double sum;
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                vSum += vSelector(new Vector<double>(InternalArray, i));
            }

            sum = Vector.Dot(vSum, Vector<double>.One);

            for (; i < Length; i++)
            {
                sum += selector(InternalArray[i]);
            }

            return sum;
        }

        public double Sum(VArray other, Func<Vector<double>, Vector<double>, Vector<double>> vSelector, Func<double, double, double> selector)
        {
            if (other.Length != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(other));
            }

            var vSum = Vector<double>.Zero;
            double sum;
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                vSum += vSelector(new Vector<double>(InternalArray, i), new Vector<double>(other.InternalArray, i));
            }

            sum = Vector.Dot(vSum, Vector<double>.One);

            for (; i < Length; i++)
            {
                sum += selector(InternalArray[i], other.InternalArray[i]);
            }

            return sum;
        }

        public double Average()
        {
            return Sum() / Length;
        }       

        public double Average(Func<Vector<double>, Vector<double>> vSelector, Func<double, double> selector)
        {
            return Sum(vSelector, selector) / Length;
        }

        public double Average(VArray other, Func<Vector<double>, Vector<double>, Vector<double>> vSelector, Func<double, double, double> selector)
        {
            return Sum(other, vSelector, selector) / Length;
        }
    }
}
