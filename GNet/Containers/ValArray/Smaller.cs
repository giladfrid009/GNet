using System.Numerics;

namespace GNet
{
    public partial class VArray<T> : Array<T> where T : unmanaged
    {
        public static bool operator <(VArray<T> left, VArray<T> right)
        {
            return left.SmallerThan(right);
        }

        public static bool operator <(VArray<T> left, T right)
        {
            return left.SmallerThan(right);
        }

        protected bool SmallerThan(T value)
        {
            var vVal = new Vector<T>(value);
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                if (Vector.GreaterThanOrEqualAll(new Vector<T>(InternalArray, i), vVal))
                {
                    return false;
                }
            }

            for (; i < Length; i++)
            {
                if (Ops.BiggerEqual(InternalArray[i], value))
                {
                    return false;
                }
            }

            return true;
        }

        protected bool SmallerThan(VArray<T> other)
        {
            int i;

            for (i = 0; i <= Length - VStride; i += VStride)
            {
                if (Vector.GreaterThanOrEqualAll(new Vector<T>(InternalArray, i), new Vector<T>(other.InternalArray, i)))
                {
                    return false;
                }
            }

            for (; i < Length; i++)
            {
                if (Ops.BiggerEqual(InternalArray[i], other.InternalArray[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
