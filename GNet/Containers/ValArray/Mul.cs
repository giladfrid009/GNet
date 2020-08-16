using System;
using System.Numerics;

namespace GNet
{
    public partial class VArray<T> : Array<T> where T : unmanaged
    {
        public static VArray<T> operator *(VArray<T> left, VArray<T> right)
        {
            return left.Mul(right);
        }

        public static VArray<T> operator *(VArray<T> left, T right)
        {
            return left.Mul(right);
        }

        protected VArray<T> Mul(T value)
        {
            var vVal = new Vector<T>(value);
            var selected = new T[Length];
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                var vCur = new Vector<T>(InternalArray, i) * vVal;
                vCur.CopyTo(selected, i);
            }

            for (; i < Length; ++i)
            {
                selected[i] = Ops.Mul(InternalArray[i], value);
            }

            return FromRef(selected);
        }

        protected VArray<T> Mul(VArray<T> other)
        {
            if (other.Length != Length)
            {
                throw new RankException(nameof(other));
            }

            var selected = new T[Length];
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                var vCur = new Vector<T>(InternalArray, i) * new Vector<T>(other.InternalArray, i);
                vCur.CopyTo(selected, i);
            }

            for (; i < Length; ++i)
            {
                selected[i] = Ops.Mul(InternalArray[i], other.InternalArray[i]);
            }

            return FromRef(selected);
        }
    }
}
