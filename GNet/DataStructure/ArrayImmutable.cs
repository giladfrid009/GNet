using System;
using System.Collections.Generic;

namespace GNet
{
    [Serializable]
    public class ArrayImmutable<T> : IArray<T>
    {
        public int Length { get; private set; }

        public T this[int i] => internalArray[i];

        private T[] internalArray;

        public ArrayImmutable()
        {
            Length = 0;

            internalArray = Array.Empty<T>();
        }

        public ArrayImmutable(params T[] elements)
        {
            Length = elements.Length;

            internalArray = new T[Length];

            Array.Copy(elements, 0, internalArray, 0, Length);
        }       

        public ArrayImmutable(IList<T> list)
        {
            Length = list.Count;

            internalArray = new T[Length];

            for (int i = 0; i < Length; i++)
            {
                internalArray[i] = list[i];
            }
        }

        public ArrayImmutable(IEnumerable<T> enumerable)
        {
            Length = System.Linq.Enumerable.Count(enumerable);

            internalArray = new T[Length];

            int i = 0;
            foreach (T x in enumerable)
            {
                internalArray[i++] = x;
            }
        }

        public ArrayImmutable(int length, Func<T> element)
        {
            Length = length;

            internalArray = new T[Length];

            for (int i = 0; i < Length; i++)
            {
                internalArray[i] = element();
            }
        }

        public static ArrayImmutable<T> FromRef(params T[] array)
        {
            return new ArrayImmutable<T>()
            {
                Length = array.Length,
                internalArray = array
            };
        }

        public T[] ToMutable()
        {
            var array = new T[Length];

            Array.Copy(internalArray, 0, array, 0, Length);

            return array;
        }

        public ShapedArrayImmutable<T> ToShape(Shape shape)
        {
            return ShapedArrayImmutable<T>.FromRef(shape, internalArray);
        }
    }
}