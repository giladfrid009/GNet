using System;
using System.Collections.Generic;

namespace GNet
{
    [Serializable]
    public class ShapedArrayImmutable<T> : ArrayImmutable<T>
    {
        public Shape Shape { get; }
        public new T this[int i] => base[i];
        public T this[params int[] idxs] => base[Shape.FlattenIndices(idxs)];

        protected ShapedArrayImmutable(Shape shape, T[] array, bool asRef = false) : base(array, asRef)
        {
            ValidateShape(shape);
            Shape = shape;
        }

        public ShapedArrayImmutable() : base()
        {
            Shape = new Shape();
        }

        public ShapedArrayImmutable(Shape shape, params T[] elements) : base(elements)
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

        public ShapedArrayImmutable(Shape shape, Func<T> element) : base(shape.Volume, element)
        {
            Shape = shape;
        }

        public static ShapedArrayImmutable<T> FromRef(Shape shape, params T[] array)
        {
            return new ShapedArrayImmutable<T>(shape, array, true);
        }

        private void ValidateShape(Shape shape)
        {
            if (shape.Volume != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(shape));
            }
        }

        public ArrayImmutable<T> Flatten()
        {
            return this;
        }
    }
}