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

        public ShapedArray(Shape shape)
        {
            internalArray = new T[shape.Volume];
            Shape = shape;
        }

        public ShapedArray(Shape shape, ArrayImmutable<T> array)
        {
            if (shape.Volume != array.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            internalArray = array.ToMutable();            
            Shape = shape;
        }

        public ShapedArray(Shape shape, params T[] array) : this(shape)
        {
            if (shape.Volume != array.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            Array.Copy(array, 0, internalArray, 0, array.Length);
        }

        public ShapedArray(Shape shape, Array array) : this(shape)
        {
            if (shape.Volume != array.Length)
            {
                throw new ArgumentException("Shape volume and array length mismatch.");
            }

            if (array.GetType().GetElementType() != typeof(T))
            {
                throw new ArgumentException();
            }

            Array.Copy(array, 0, internalArray, 0, array.Length);
        }

        public ShapedArray(Shape shape, IList<T> list) : this(shape)
        {
            if (shape.Volume != list.Count)
            {
                throw new ArgumentException("Shape volume and list length mismatch.");
            }

            int length = list.Count;

            for (int i = 0; i < length; i++)
            {
                internalArray[i] = list[i];
            }
        }

        public ShapedArray(Shape shape, IEnumerable<T> enumerable) : this(shape)
        {
            if (shape.Volume != System.Linq.Enumerable.Count(enumerable))
            {
                throw new ArgumentException("Shape volume and enumerable length mismatch.");
            }

            int i = 0;
            foreach (var x in enumerable)
            {
                internalArray[i++] = x;
            }
        }

        public ShapedArray(Shape shape, Func<T> element) : this(shape)
        {
            for (int i = 0; i < Length; i++)
            {
                internalArray[i] = element();
            }
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
