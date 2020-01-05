using System;
using System.Collections.Generic;

namespace GNet
{
    [Serializable]
    public class ShapedArray<T> : IShapedArray<T>, ICloneable<ShapedArray<T>>
    {
        public Shape Shape { get; }
        public int Length => internalArray.Length;
        
        public T this[int index]
        {
            get => internalArray[index];
            set => internalArray[index] = value;
        }

        public T this[params int[] indices]
        {
            get => internalArray[Shape.FlattenIndices(indices)];
            set => internalArray[Shape.FlattenIndices(indices)] = value;
        }

        private readonly T[] internalArray;

        public ShapedArray(Shape shape, ArrayImmutable<T> array)
        {
            internalArray = array.ToMutable();

            if (shape.Volume != internalArray.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            Shape = shape;
        }

        public ShapedArray(Shape shape, params T[] array)
        {
            internalArray = new T[array.Length];

            Array.Copy(array, 0, internalArray, 0, array.Length);

            if (shape.Volume != internalArray.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            Shape = shape;
        }

        public ShapedArray(Shape shape, Array array)
        {
            if (array.GetType().GetElementType() != typeof(T))
            {
                throw new ArgumentException();
            }

            internalArray = new T[array.Length];

            Array.Copy(array, 0, internalArray, 0, array.Length);

            if (shape.Volume != internalArray.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            Shape = shape;
        }

        public ShapedArray(Shape shape, IList<T> list)
        {
            int length = list.Count;

            internalArray = new T[length];

            for (int i = 0; i < length; i++)
            {
                internalArray[i] = list[i];
            }

            if (shape.Volume != internalArray.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            Shape = shape;
        }

        public ShapedArray(Shape shape, IEnumerable<T> enumerable)
        {
            int length = System.Linq.Enumerable.Count(enumerable);

            internalArray = new T[length];

            int i = 0;
            foreach (var x in enumerable)
            {
                internalArray[i++] = x;
            }

            if (shape.Volume != internalArray.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            Shape = shape;
        }

        public ShapedArrayImmutable<T> ToImmutable()
        {
            return new ShapedArrayImmutable<T>(Shape, internalArray);
        }

        public ShapedArray<T> Clone()
        {
            return new ShapedArray<T>(Shape, internalArray);
        }
    }
}
