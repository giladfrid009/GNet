using System;
using System.Collections;
using System.Collections.Generic;

namespace GNet
{
    [Serializable]
    public class ShapedArrayImmutable<T> : ArrayImmutable<T>
    {
        public Shape Shape { get; }
        public new T this[int i] => internalArray[i];
        public T this[params int[] idxs] => internalArray[Shape.FlattenIndices(idxs)];

        public ShapedArrayImmutable() : base()
        {
            Shape = new Shape();
        }

        public ShapedArrayImmutable(Shape shape, params T[] array) : base(array)
        {
            ValidateShape(shape);
            Shape = shape;
        }

        public ShapedArrayImmutable(Shape shape, in T[] array) : base(in array)
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

        public ArrayImmutable<T> Flatten()
        {
            return this;
        }
    }
}