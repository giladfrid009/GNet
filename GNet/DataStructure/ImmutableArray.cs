using System;
using System.Collections.Generic;

namespace GNet
{
    [Serializable]
    public readonly struct ImmutableArray<T> : IEquatable<ImmutableArray<T>>
    {
        public int Length { get; }

        public T this[int i] => internalArray[i];

        private readonly T[] internalArray;

        private ImmutableArray(T[] array, bool asRef = false)
        {
            Length = array.Length;

            if (asRef)
            {
                internalArray = array;
            }
            else
            {
                internalArray = new T[Length];

                Array.Copy(array, 0, internalArray, 0, Length);
            }
        }

        public ImmutableArray(params T[] elements) : this(elements, false)
        {
        }

        public ImmutableArray(int length, Func<T> element)
        {
            Length = length;

            internalArray = new T[Length];

            for (int i = 0; i < Length; i++)
            {
                internalArray[i] = element();
            }
        }

        public static bool operator !=(in ImmutableArray<T> left, in ImmutableArray<T> right)
        {
            return !(left == right);
        }

        public static bool operator ==(in ImmutableArray<T> left, in ImmutableArray<T> right)
        {
            return left.Equals(right);
        }

        public bool Equals(ImmutableArray<T> other)
        {
            if (ReferenceEquals(internalArray, other.internalArray))
            {
                return true;
            }

            if (Length != other.Length)
            {
                return false;
            }         

            var comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < Length; i++)
            {
                if (comparer.Equals(internalArray[i], other.internalArray[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            return (obj is ImmutableArray<T> immArr) && Equals(immArr);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(internalArray, Length);
        }

        public static ImmutableArray<T> FromRef(params T[] array)
        {
            return new ImmutableArray<T>(array, true);
        }

        public T[] ToMutable()
        {
            var array = new T[Length];

            Array.Copy(internalArray, 0, array, 0, Length);

            return array;
        }

        public ImmutableShapedArray<T> ToShape(in Shape shape)
        {
            return ImmutableShapedArray<T>.FromRef(shape, internalArray);
        }
    }
}