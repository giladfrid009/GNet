using System;

namespace GNet
{
    [Serializable]
    public class ShapedArray<T> : IArray<T>, ICloneable<ShapedArray<T>>
    {
        public Shape Shape { get; }

        protected readonly T[] array;

        public int Length { get; }

        public T this[params int[] indices]
        {
            get => array[Shape.FlattenIndices(indices)];
        }

        public T this[int index]
        {
            get => array[index];
        }

        public ShapedArray(Shape shape)
        {
            Shape = shape;
            Length = shape.Volume;
            array = new T[Length];
        }

        public ShapedArray(Shape shape, Array array) : this(shape)
        {
            if (array.Length != Length)
            {
                throw new ArgumentException("array length and shape mismatch.");
            }

            Array.Copy(array, 0, this.array, 0, Length);
        }

        public ShapedArray(Shape shape, params T[] array) : this(shape, (Array)array)
        {
            if (array.Length != Length)
            {
                throw new ArgumentException("array length and shape mismatch.");
            }

            Array.Copy(array, 0, this.array, 0, Length);
        }

        public ShapedArray<T> Clone()
        {
            return new ShapedArray<T>(Shape, array);
        }
    }
}
