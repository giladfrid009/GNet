using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GNet
{
    [Serializable]
    public struct ArrayImmutable<T> : IArray<T>, IEquatable<ArrayImmutable<T>>
    {
        public int Length => internalArray.Length;

        public T this[int index] => internalArray[index];

        private readonly T[] internalArray;

        public ArrayImmutable(ArrayImmutable<T> array)
        {
            this = array;
        }

        public ArrayImmutable(T[] array)
        {
            internalArray = new T[array.Length];

            Array.Copy(array, 0, internalArray, 0, array.Length);
        }

        public ArrayImmutable(Array array)
        {
            if(array.GetType().GetElementType() != typeof(T))
            {
                throw new ArgumentException();
            }

            internalArray = new T[array.Length];

            Array.Copy(array, 0, internalArray, 0, array.Length);
        }

        public ArrayImmutable(IList<T> list)
        {
            int length = list.Count;

            internalArray = new T[length];

            for (int i = 0; i < length; i++)
            {
                internalArray[i] = list[i];
            }
        }

        public ArrayImmutable(IEnumerable<T> enumerable)
        {
            int length = System.Linq.Enumerable.Count(enumerable);

            internalArray = new T[length];

            int i = 0;
            foreach (var x in enumerable)
            {
                internalArray[i++] = x;
            }
        }

        public bool Equals([AllowNull] ArrayImmutable<T> other)
        {
            if (other == null)
                return false;

            if (other.Length != Length)
                return false;

            for (int i = 0; i < Length; i++)
            {
                if (internalArray[i] == null && other.internalArray[i] != null)
                {
                    return false;
                }

                if (internalArray[i]?.Equals(other.internalArray[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj);
        }     

        public static bool operator ==(ArrayImmutable<T> left, ArrayImmutable<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ArrayImmutable<T> left, ArrayImmutable<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return internalArray.GetHashCode() + Length * 13;
        }
    }
}
