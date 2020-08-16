using System;
using System.Numerics;

namespace GNet
{
    public partial class VArray<T> : Array<T> where T : unmanaged
    {
        public static bool operator ==(VArray<T> left, VArray<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VArray<T> left, VArray<T> right)
        {
            return !(left == right);
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

            for (i = 0; i < Length - VStride; i += VStride)
            {
                if (new Vector<T>(InternalArray, i) != new Vector<T>(other.InternalArray, i))
                {
                    return false;
                }
            }

            for (; i < Length; i++)
            {
                if (Ops.Equals(InternalArray[i], other.InternalArray[i]) == false)
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
            return HashCode.Combine(InternalArray, Length, VStride);
        }
    }
}
