using System;
using System.Collections;
using System.Collections.Generic;

namespace GNet
{
    [Serializable]
    public class ArrayImmutable<T> : IArray<T>
    {
        public int Length => internalArray.Length;
        public T this[int i] => internalArray[i];

        protected readonly T[] internalArray;

        public ArrayImmutable(params T[] array)
        {
            internalArray = new T[array.Length];

            Array.Copy(array, 0, internalArray, 0, array.Length);
        }

        public ArrayImmutable(in T[] array)
        {
            internalArray = array;
        }

        public ArrayImmutable(IList<T> list)
        {
            int length = list.Count;

            internalArray = new T[length];

            for (int i = 0; i < length; i++)
            {
                internalArray[i] = list[i];
            }
        }

        public ArrayImmutable(IEnumerable<T> enumerable)
        {
            int length = System.Linq.Enumerable.Count(enumerable);

            internalArray = new T[length];

            int i = 0;
            foreach (T x in enumerable)
            {
                internalArray[i++] = x;
            }
        }

        public ArrayImmutable(int length, Func<T> element)
        {
            internalArray = new T[length];

            for (int i = 0; i < Length; i++)
            {
                internalArray[i] = element();
            }
        }

        public T[] ToMutable()
        {
            var array = new T[Length];

            Array.Copy(internalArray, 0, array, 0, Length);

            return array;
        }

        public ShapedArrayImmutable<T> ToShape(Shape shape)
        {
            return new ShapedArrayImmutable<T>(shape, in internalArray);
        }
    }
}