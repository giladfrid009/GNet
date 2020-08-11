using System;
using System.Numerics;

namespace GNet
{
    [Serializable]
    public class VArray : Array<double>
    {
        private static readonly int vStride = Vector<double>.Count;

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

            for (i = 0; i <= Length - vStride; i += vStride)
            {
                var vCur = vSelector(new Vector<double>(internalArray, i), i);
                vCur.CopyTo(selected, i);
            }

            for (; i < Length; ++i)
            {
                selected[i] = selector(internalArray[i], i);
            }

            return FromRef(selected);
        }

        public double Min()
        {
            var vMin = new Vector<double>(double.MaxValue);
            double min = double.MaxValue;
            int i;

            for (i = 0; i <= Length - vStride; i += vStride)
            {
                var vCur = new Vector<double>(internalArray, i);
                vMin = Vector.Min(vMin, vCur);
            }

            for (var j = 0; j < vStride; ++j)
            {
                min = Math.Min(min, vMin[j]);
            }

            for (; i < Length; ++i)
            {
                min = Math.Min(min, internalArray[i]);
            }

            return min;
        }

        public double Min(Func<Vector<double>, Vector<double>> vSelector, Func<double, double> selector)
        {
            var vMin = new Vector<double>(double.MaxValue);
            double min = double.MaxValue;
            int i;

            for (i = 0; i <= Length - vStride; i += vStride)
            {
                var vCur = vSelector(new Vector<double>(internalArray, i));
                vMin = Vector.Min(vMin, vCur);
            }

            for (var j = 0; j < vStride; ++j)
            {
                min = Math.Min(min, vMin[j]);
            }

            for (; i < Length; ++i)
            {
                min = Math.Min(min, selector(internalArray[i]));
            }

            return min;
        }

        public double Max()
        {
            var vMax = new Vector<double>(double.MinValue);
            double max = double.MinValue;
            int i;

            for (i = 0; i <= Length - vStride; i += vStride)
            {
                var vec = new Vector<double>(internalArray, i);
                vMax = Vector.Max(vMax, vec);
            }

            for (var j = 0; j < vStride; ++j)
            {
                max = Math.Max(max, vMax[j]);
            }

            for (; i < Length; ++i)
            {
                max = Math.Max(max, internalArray[i]);
            }

            return max;
        }

        public double Max(Func<Vector<double>, Vector<double>> vSelector, Func<double, double> selector)
        {
            var vMax = new Vector<double>(double.MinValue);
            double max = double.MinValue;
            int i;

            for (i = 0; i <= Length - vStride; i += vStride)
            {
                var vCur = vSelector(new Vector<double>(internalArray, i));
                vMax = Vector.Max(vMax, vCur);
            }

            for (var j = 0; j < vStride; ++j)
            {
                max = Math.Max(max, vMax[j]);
            }

            for (; i < Length; ++i)
            {
                max = Math.Max(max, selector(internalArray[i]));
            }

            return max;
        }

        public double Sum()
        {
            var vSum = Vector<double>.Zero;
            double sum;
            int i;

            for (i = 0; i <= Length - vStride; i += vStride)
            {
                vSum += new Vector<double>(internalArray, i);
            }

            sum = Vector.Dot(vSum, Vector<double>.One);

            for (; i < Length; i++)
            {
                sum += internalArray[i];
            }

            return sum;
        }     

        public double Sum(Func<Vector<double>, Vector<double>> vSelector, Func<double, double> selector)
        {
            var vSum = Vector<double>.Zero;
            double sum;
            int i;

            for (i = 0; i <= Length - vStride; i += vStride)
            {
                vSum += vSelector(new Vector<double>(internalArray, i));
            }

            sum = Vector.Dot(vSum, Vector<double>.One);

            for (; i < Length; i++)
            {
                sum += selector(internalArray[i]);
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

            for (i = 0; i <= Length - vStride; i += vStride)
            {
                vSum += vSelector(new Vector<double>(internalArray, i), new Vector<double>(other.internalArray, i));
            }

            sum = Vector.Dot(vSum, Vector<double>.One);

            for (; i < Length; i++)
            {
                sum += selector(internalArray[i], other.internalArray[i]);
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
