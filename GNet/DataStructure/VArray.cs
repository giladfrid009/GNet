using System;
using System.Numerics;
using GNet.DataStructure.NumOps;

namespace GNet
{
    [Serializable]
    public class VArray<T> : Array<T> where T : unmanaged
    {
        protected static INumOps<T> Ops { get; }
        protected static int VecStride { get; }

        static VArray()
        {
            if(typeof(T) == typeof(int))
            {
                Ops = (INumOps<T>)new IntOps();
            }
            else if (typeof(T) == typeof(float))
            {
                Ops = (INumOps<T>)new FloatOps();
            }
            else if (typeof(T) == typeof(double))
            {
                Ops = (INumOps<T>)new DoubleOps();
            }
            else
            {
                throw new NotSupportedException(typeof(T).Name);
            }

            VecStride = Vector<T>.Count;
        }

        protected VArray(T[] vals, bool asRef = false) : base(vals, asRef)
        {
        }

        public VArray(params T[] vals) : this(vals, false)
        {
        }

        public VArray(int length, Func<T> element) : base(length, element)
        {
        }

        public static new VArray<T> FromRef(params T[] array)
        {
            return new VArray<T>(array, true);
        }

        public VArray<T> Select(Func<Vector<T>, Vector<T>> vSelector, Func<T, T> selector)
        {
            var selected = new T[Length];
            int i;

            for (i = 0; i <= Length - VecStride; i += VecStride)
            {
                var vCur = vSelector(new Vector<T>(internalArray, i));
                vCur.CopyTo(selected, i);
            }

            for (; i < Length; ++i)
            {
                selected[i] = selector(internalArray[i]);
            }

            return FromRef(selected);
        }

        public VArray<T> Select(VArray<T> other, Func<Vector<T>, Vector<T>, Vector<T>> vSelector, Func<T, T, T> selector)
        {
            if(other.Length != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(other));
            }

            var selected = new T[Length];
            int i;

            for (i = 0; i <= Length - VecStride; i += VecStride)
            {
                var vCur = vSelector(new Vector<T>(internalArray, i), new Vector<T>(other.internalArray, i));
                vCur.CopyTo(selected, i);
            }

            for (; i < Length; ++i)
            {
                selected[i] = selector(internalArray[i], other.internalArray[i]);
            }

            return FromRef(selected);
        }

        public T Min()
        {
            var vMin = new Vector<T>(Ops.MaxValue);
            T min = Ops.MaxValue;
            int i;

            for (i = 0; i <= Length - VecStride; i += VecStride)
            {
                var vCur = new Vector<T>(internalArray, i);
                vMin = Vector.Min(vMin, vCur);
            }

            for (var j = 0; j < VecStride; ++j)
            {
                min = Ops.Min(min, vMin[j]);
            }

            for (; i < Length; ++i)
            {
                min = Ops.Min(min, internalArray[i]);
            }

            return min;
        }

        public T Min(Func<Vector<T>, Vector<T>> vSelector, Func<T, T> selector)
        {
            var vMin = new Vector<T>(Ops.MaxValue);
            T min = Ops.MaxValue;
            int i;

            for (i = 0; i <= Length - VecStride; i += VecStride)
            {
                var vCur = vSelector(new Vector<T>(internalArray, i));
                vMin = Vector.Min(vMin, vCur);
            }

            for (var j = 0; j < VecStride; ++j)
            {
                min = Ops.Min(min, vMin[j]);
            }

            for (; i < Length; ++i)
            {
                min = Ops.Min(min, selector(internalArray[i]));
            }

            return min;
        }

        public T Max()
        {
            var vMax = new Vector<T>(Ops.MinValue);
            T max = Ops.MinValue;
            int i;

            for (i = 0; i <= Length - VecStride; i += VecStride)
            {
                var vec = new Vector<T>(internalArray, i);
                vMax = Vector.Max(vMax, vec);
            }

            for (var j = 0; j < VecStride; ++j)
            {
                max = Ops.Max(max, vMax[j]);
            }

            for (; i < Length; ++i)
            {
                max = Ops.Max(max, internalArray[i]);
            }

            return max;
        }

        public T Max(Func<Vector<T>, Vector<T>> vSelector, Func<T, T> selector)
        {
            var vMax = new Vector<T>(Ops.MinValue);
            T max = Ops.MinValue;
            int i;

            for (i = 0; i <= Length - VecStride; i += VecStride)
            {
                var vCur = vSelector(new Vector<T>(internalArray, i));
                vMax = Vector.Max(vMax, vCur);
            }

            for (var j = 0; j < VecStride; ++j)
            {
                max = Ops.Max(max, vMax[j]);
            }

            for (; i < Length; ++i)
            {
                max = Ops.Max(max, selector(internalArray[i]));
            }

            return max;
        }

        public T Sum()
        {
            var vSum = Vector<T>.Zero;
            T sum;
            int i;

            for (i = 0; i <= Length - VecStride; i += VecStride)
            {
                vSum += new Vector<T>(internalArray, i);
            }

            sum = Vector.Dot(vSum, Vector<T>.One);

            for (; i < Length; i++)
            {
                sum = Ops.Add(sum, internalArray[i]);
            }

            return sum;
        }

        public T Sum(Func<Vector<T>, Vector<T>> vSelector, Func<T, T> selector)
        {
            var vSum = Vector<T>.Zero;
            T sum;
            int i;

            for (i = 0; i <= Length - VecStride; i += VecStride)
            {
                vSum += vSelector(new Vector<T>(internalArray, i));
            }

            sum = Vector.Dot(vSum, Vector<T>.One);

            for (; i < Length; i++)
            {
                sum = Ops.Add(sum, selector(internalArray[i]));
            }

            return sum;
        }

        public T Sum(VArray<T> other, Func<Vector<T>, Vector<T>, Vector<T>> vSelector, Func<T, T, T> selector)
        {
            if (other.Length != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(other));
            }

            var vSum = Vector<T>.Zero;
            T sum;
            int i;

            for (i = 0; i <= Length - VecStride; i += VecStride)
            {
                vSum += vSelector(new Vector<T>(internalArray, i), new Vector<T>(other.internalArray, i));
            }

            sum = Vector.Dot(vSum, Vector<T>.One);

            for (; i < Length; i++)
            {
                sum = Ops.Add(sum, selector(internalArray[i], other.internalArray[i]));
            }

            return sum;
        }

        public T Average()
        {
            return Ops.Div(Sum(), Ops.From(Length));
        }

        public T Average(Func<Vector<T>, Vector<T>> vSelector, Func<T, T> selector)
        {
            return Ops.Div(Sum(vSelector, selector), Ops.From(Length));
        }

        public T Average(VArray<T> other, Func<Vector<T>, Vector<T>, Vector<T>> vSelector, Func<T, T, T> selector)
        {
            return Ops.Div(Sum(other, vSelector, selector), Ops.From(Length));
        }

        public bool Equals(VArray<T>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (Length != other.Length)
            {
                return false;
            }

            int i;

            for (i = 0; i < Length - VecStride; i += VecStride)
            {
                if (new Vector<T>(internalArray, i) != new Vector<T>(other.internalArray, i))
                {
                    return false;
                }
            }

            for (; i < Length; i++)
            {
                if (Ops.Equals(internalArray[i], other.internalArray[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as VArray<T>);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(internalArray, Length, VecStride);
        }
    }
}
