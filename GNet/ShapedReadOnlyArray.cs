using System;

namespace GNet
{
    [Serializable]
    public class ShapedReadOnlyArray<T> : IArray<T>, ICloneable<ShapedReadOnlyArray<T>>
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

        public ShapedReadOnlyArray(Shape shape)
        {
            Shape = shape.Clone();
            Length = shape.Length();
            array = new T[Length];
        }

        public ShapedReadOnlyArray(Shape shape, Array array) : this(shape)
        {
            if (array.Length != Length)
            {
                throw new ArgumentException("array length and shape mismatch.");
            }

            Array.Copy(array, 0, this.array, 0, Length);
        }

        public ShapedReadOnlyArray(Shape shape, params T[] array) : this(shape, (Array)array)
        {
            if (array.Length != Length)
            {
                throw new ArgumentException("array length and shape mismatch.");
            }

            Array.Copy(array, 0, this.array, 0, Length);
        }

        public ShapedReadOnlyArray<T> Clone()
        {
            return new ShapedReadOnlyArray<T>(Shape, array);
        }
    }
}
