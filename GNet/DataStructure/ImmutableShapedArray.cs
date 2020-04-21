using System;

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
            if (shape.Volume != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(shape), nameof(shape.Volume));
            }

            Shape = shape;
        }

        public ImmutableShapedArray(params T[] elements) : this(new Shape(elements.Length), elements, false)
        {
        }

        public ImmutableShapedArray(Shape shape, params T[] elements) : this(shape, elements, false)
        {
        }

        public ImmutableShapedArray(Shape shape, Func<T> element) : base(shape.Volume, element)
        {
            Shape = shape;
        }

        public static ImmutableShapedArray<T> FromRef(Shape shape, params T[] array)
        {
            return new ImmutableShapedArray<T>(shape, array, true);
        }

        public ImmutableArray<T> Flatten()
        {
            return this;
        }
    }
}