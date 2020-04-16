using System;
using System.Collections.Generic;

namespace GNet
{
    [Serializable]
    public class ImmutableShapedArray<T> : ImmutableArray<T>
    {
        public Shape Shape { get; }
        public new T this[int i] => base[i];
        public T this[params int[] idxs] => base[Shape.FlattenIndices(idxs)];

        protected ImmutableShapedArray(Shape shape, T[] array, bool asRef = false) : base(array, asRef)
        {
            ValidateShape(shape);
            Shape = shape;
        }

        public ImmutableShapedArray() : base()
        {
            Shape = new Shape();
        }

        public ImmutableShapedArray(Shape shape, params T[] elements) : base(elements)
        {
            ValidateShape(shape);
            Shape = shape;
        }

        public ImmutableShapedArray(Shape shape, IList<T> list) : base(list)
        {
            ValidateShape(shape);
            Shape = shape;
        }

        public ImmutableShapedArray(Shape shape, IEnumerable<T> enumerable) : base(enumerable)
        {
            ValidateShape(shape);
            Shape = shape;
        }

        public ImmutableShapedArray(Shape shape, Func<T> element) : base(shape.Volume, element)
        {
            Shape = shape;
        }

        public static ImmutableShapedArray<T> FromRef(Shape shape, params T[] array)
        {
            return new ImmutableShapedArray<T>(shape, array, true);
        }

        private void ValidateShape(Shape shape)
        {
            if (shape.Volume != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(shape));
            }
        }

        public ImmutableArray<T> Flatten()
        {
            return this;
        }
    }
}