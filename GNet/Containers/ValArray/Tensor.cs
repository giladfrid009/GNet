using System;

namespace GNet
{
    [Serializable]
    public class Tensor<T> : VArray<T> where T : unmanaged
    {
        public Shape Shape { get; }
        public T this[int[] idxs] => InternalArray[Shape.FlattenIndices(idxs)];

        protected Tensor(Shape shape, T[] array, bool asRef = false) : base(array, asRef)
        {
            if (shape.Volume != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(shape), nameof(shape.Volume));
            }

            Shape = shape;
        }

        public Tensor(params T[] elements) : this(new Shape(elements.Length), elements, false)
        {
        }

        public Tensor(Shape shape, params T[] elements) : this(shape, elements, false)
        {
        }

        public Tensor(Shape shape, Array<T> array) : base(array)
        {
            if (shape.Volume != Length)
            {
                throw new ArgumentOutOfRangeException(nameof(shape), nameof(shape.Volume));
            }

            Shape = shape;
        }

        public Tensor(Shape shape, Func<T> element) : base(shape.Volume, element)
        {
            Shape = shape;
        }

        public static Tensor<T> FromRef(Shape shape, params T[] array)
        {
            return new Tensor<T>(shape, array, true);
        }
    }
}