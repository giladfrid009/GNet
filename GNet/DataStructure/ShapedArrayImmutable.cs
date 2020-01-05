using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GNet
{
    [Serializable]
    public struct ShapedArrayImmutable<T> : IShapedArray<T>, IEquatable<ShapedArrayImmutable<T>>
    {
        public Shape Shape { get; }
        public int Length => internalArray.Length;     
        public T this[int index] => internalArray[index];
        public T this[params int[] indices] => internalArray[Shape.FlattenIndices(indices)];

        private readonly ArrayImmutable<T> internalArray;

        public ShapedArrayImmutable(Shape shape, ArrayImmutable<T> array)
        {
            internalArray = array;

            if(shape.Volume != internalArray.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            Shape = shape;
        }

        public ShapedArrayImmutable(Shape shape, params T[] array)
        {
            internalArray = new ArrayImmutable<T>(array);

            if (shape.Volume != internalArray.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            Shape = shape;
        }

        public ShapedArrayImmutable(Shape shape, Array array)
        {
            internalArray = new ArrayImmutable<T>(array);

            if (shape.Volume != internalArray.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            Shape = shape;
        }

        public ShapedArrayImmutable(Shape shape, IList<T> list)
        {
            internalArray = new ArrayImmutable<T>(list);

            if (shape.Volume != internalArray.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            Shape = shape;
        }

        public ShapedArrayImmutable(Shape shape, IEnumerable<T> enumerable)
        {
            internalArray = new ArrayImmutable<T>(enumerable);

            if (shape.Volume != internalArray.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            Shape = shape;
        }

        public bool Equals([AllowNull] ShapedArrayImmutable<T> other)
        {
            if (other == null)
                return false;

            return (Shape, internalArray) == (other.Shape, other.internalArray);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj);
        }

        public static bool operator ==(ShapedArrayImmutable<T> left, ShapedArrayImmutable<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ShapedArrayImmutable<T> left, ShapedArrayImmutable<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return internalArray.GetHashCode() + Shape.GetHashCode();
        }
    }
}
