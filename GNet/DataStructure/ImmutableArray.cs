using System;
using System.Collections.Generic;

namespace GNet
{
    [Serializable]
    public class ImmutableArray<T> : IArray<T>
    {
        public int Length { get; }

        public T this[int i] => internalArray[i];

        private readonly T[] internalArray;

        protected ImmutableArray(T[] array, bool asRef = false)
        {
            Length = array.Length;

            if (asRef)
            {
                internalArray = array;
            }
            else
            {
                internalArray = new T[Length];

                Array.Copy(array, 0, internalArray, 0, Length);
            }
        }

        public ImmutableArray()
        {
            Length = 0;

            internalArray = Array.Empty<T>();
        }

        public ImmutableArray(params T[] elements) : this(elements, false)
        {
        }

        public ImmutableArray(IList<T> list)
        {
            Length = list.Count;

            internalArray = new T[Length];

            for (int i = 0; i < Length; i++)
            {
                internalArray[i] = list[i];
            }
        }

        public ImmutableArray(IEnumerable<T> enumerable)
        {
            Length = System.Linq.Enumerable.Count(enumerable);

            internalArray = new T[Length];

            int i = 0;
            foreach (T x in enumerable)
            {
                internalArray[i++] = x;
            }
        }

        public ImmutableArray(int length, Func<T> element)
        {
            Length = length;

            internalArray = new T[Length];

            for (int i = 0; i < Length; i++)
            {
                internalArray[i] = element();
            }
        }

        public static ImmutableArray<T> FromRef(params T[] array)
        {
            return new ImmutableArray<T>(array, true);
        }

        public T[] ToMutable()
        {
            var array = new T[Length];

            Array.Copy(internalArray, 0, array, 0, Length);

            return array;
        }

        public ImmutableShapedArray<T> ToShape(Shape shape)
        {
            return ImmutableShapedArray<T>.FromRef(shape, internalArray);
        }
    }
}