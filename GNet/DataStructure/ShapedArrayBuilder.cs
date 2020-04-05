using System;
using System.Collections.Generic;

namespace GNet
{
    public class ShapedArrayBuilder<T> : ArrayBuilder<T>
    {
        public Shape Shape { get; set; }

        public new T this[int i]
        {
            get => base[i];
            set => base[i] = value;
        }

        public T this[params int[] idxs]
        {
            get => base[Shape.FlattenIndices(idxs)];
            set => base[Shape.FlattenIndices(idxs)] = value;
        }

        public ShapedArrayBuilder() : base()
        {
            Shape = new Shape(0);
        }

        public ShapedArrayBuilder(Shape shape) : base(shape.Volume)
        {
            Shape = shape;
        }

        public ShapedArrayBuilder(Shape shape, params T[] elements) : base(elements)
        {
            ValidateShape(shape);
            Shape = shape;
        }

        public ShapedArrayBuilder(Shape shape, in List<T> list) : base(in list)
        {
            ValidateShape(shape);
            Shape = shape;
        }

        public ShapedArrayBuilder(Shape shape, IList<T> list) : base(list)
        {
            ValidateShape(shape);
            Shape = shape;
        }

        public ShapedArrayBuilder(Shape shape, IEnumerable<T> enumerable) : base(enumerable)
        {
            ValidateShape(shape);
            Shape = shape;
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

        public ArrayBuilder<T> Flatten()
        {
            return this;
        }
    }
}
