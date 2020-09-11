using System;

namespace GNet
{
    [Serializable]
    public class ShapedArray<T> : Array<T>
    {
        public Shape Shape { get; }
        public T this[int[] idxs] => internalArray[Shape.FlattenIndices(idxs)];

        protected ShapedArray(Shape shape, T[] array, bool asRef = false) : base(array, asRef)
        {
            if (shape.Volume != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(shape), nameof(shape.Volume));
            }

            Shape = shape;
        }

        public ShapedArray(params T[] elements) : this(new Shape(elements.Length), elements, false)
        {
        }

        public ShapedArray(Shape shape, params T[] elements) : this(shape, elements, false)
        {
        }

        public ShapedArray(Shape shape, Func<T> element) : base(shape.Volume, element)
        {
            Shape = shape;
        }

        public static ShapedArray<T> FromRef(Shape shape, params T[] array)
        {
            return new ShapedArray<T>(shape, array, true);
        }
    }
}