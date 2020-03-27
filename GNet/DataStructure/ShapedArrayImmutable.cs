using System;
using System.Collections;
using System.Collections.Generic;

namespace GNet
{
    [Serializable]
    public class ShapedArrayImmutable<T> : ArrayImmutable<T>, IEquatable<ShapedArrayImmutable<T>>
    {
        public Shape Shape { get; }
        public new T this[int index] => base[index];
        public T this[params int[] indices] => this[Shape.FlattenIndices(indices)];

        public ShapedArrayImmutable() : base()
        {
            Shape = new Shape(0);
        }

        public ShapedArrayImmutable(Shape shape, ArrayImmutable<T> array) : base(array)
        {
            ValidateShape(shape);
            Shape = shape;
        }

        public ShapedArrayImmutable(Shape shape, params T[] array) : base(array)
        {
            ValidateShape(shape);
            Shape = shape;
        }       

        public ShapedArrayImmutable(Shape shape, IList<T> list) : base(list)
        {
            ValidateShape(shape);
            Shape = shape;
        }   

        public ShapedArrayImmutable(Shape shape, IEnumerable<T> enumerable) : base(enumerable)
        {
            ValidateShape(shape);
            Shape = shape;
        }

        public ShapedArrayImmutable(Shape shape, IEnumerable enumerable) : base(enumerable)
        {
            ValidateShape(shape);
            Shape = shape;
        }

        public ShapedArrayImmutable(Shape shape, Func<T> element) : base(shape.Volume, element)
        {
            Shape = shape;
        }

        private void ValidateShape(Shape shape)
        {
            if (shape.Volume != Length)
            {
                throw new ArgumentException("Shape volume and length mismatch.");
            }
        }

        public static bool operator !=(ShapedArrayImmutable<T> left, ShapedArrayImmutable<T> right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(ShapedArrayImmutable<T> left, ShapedArrayImmutable<T> right)
        {
            return left.Equals(right);
        }

        public bool Equals(ShapedArrayImmutable<T> other)
        {
            if (Shape != other.Shape)
            {
                return false;
            }

            return base.Equals(other);
        }

        public override bool Equals(object? obj)
        {
            return (obj is ShapedArrayImmutable<T> shapedArr) && Equals(shapedArr);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + Shape.GetHashCode();
        }

        public ArrayImmutable<T> ToFlat()
        {
            return new ArrayImmutable<T>(this);
        }
    }
}