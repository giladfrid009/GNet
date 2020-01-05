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
            if (shape.Volume != array.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            internalArray = array;
            Shape = shape;
        }

        public ShapedArrayImmutable(Shape shape, params T[] array) : this(shape, new ArrayImmutable<T>(array))
        {

        }

        public ShapedArrayImmutable(Shape shape, Array array) : this(shape, new ArrayImmutable<T>(array))
        {

        }

        public ShapedArrayImmutable(Shape shape, IList<T> list) : this(shape, new ArrayImmutable<T>(list))
        {

        }

        public ShapedArrayImmutable(Shape shape, IEnumerable<T> enumerable) : this(shape, new ArrayImmutable<T>(enumerable))
        {

        }

        public ShapedArrayImmutable(Shape shape, Func<T> element) : this(shape, new ArrayImmutable<T>(shape.Volume, element))
        {

        }

        public bool Equals([AllowNull] ShapedArrayImmutable<T> other)
        {
            if (other == null)
            {
                return false;
            }

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
