using System;
using System.Collections.Generic;

namespace GNet
{
    public class ShapedArrayBuilder<T> : ArrayBuilder<T>
    {
        public Shape Shape { get; set; }

        public new T this[int i]
        {
            get => internalList[i];
            set => internalList[i] = value;
        }

        public T this[params int[] idxs]
        {
            get => internalList[Shape.FlattenIndices(idxs)];
            set => internalList[Shape.FlattenIndices(idxs)] = value;
        }

        public ShapedArrayBuilder(Shape shape) : base(shape.Volume)
        {
            Shape = shape;
        }

        public ShapedArrayBuilder(params T[] array) : base(array)
        {
            ValidateShape(Shape);
            Shape = Shape;
        }

        public ShapedArrayBuilder(IList<T> list) : base(list)
        {
            ValidateShape(Shape);
            Shape = Shape;
        }

        public ShapedArrayBuilder(IEnumerable<T> enumerable) : base(enumerable)
        {
            ValidateShape(Shape);
            Shape = Shape;
        }

        public ShapedArrayBuilder(Shape shape, Func<T> element) : base(shape.Volume, element)
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
    }
}
