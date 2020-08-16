using System;
using System.Numerics;

namespace GNet
{
    public partial class VArray<T> : Array<T> where T : unmanaged
    {
        public static VArray<T> operator +(VArray<T> left, VArray<T> right)
        {
            return left.Add(right);
        }

        public static VArray<T> operator +(VArray<T> left, T right)
        {
            return left.Add(right);
        }

        protected VArray<T> Add(T value)
        {
            var vVal = new Vector<T>(value);
            var selected = new T[Length];
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                var vCur = new Vector<T>(internalArray, i) + vVal;
                vCur.CopyTo(selected, i);
            }

            for (; i < Length; ++i)
            {
                selected[i] = Ops.Add(internalArray[i], value);
            }

            return FromRef(selected);
        }

        protected VArray<T> Add(VArray<T> other)
        {
            if (other.Length != Length)
            {
                throw new RankException(nameof(other));
            }

            var selected = new T[Length];
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                var vCur = new Vector<T>(internalArray, i) + new Vector<T>(other.internalArray, i);
                vCur.CopyTo(selected, i);
            }

            for (; i < Length; ++i)
            {
                selected[i] = Ops.Add(internalArray[i], other.internalArray[i]);
            }

            return FromRef(selected);
        }
    }
}
