using System;

namespace GNet
{
    [Serializable]
    public class Array<T> : IArray<T>
    {
        public int Length { get; }

        public T this[int i] => internalArray[i];

        private readonly T[] internalArray;

        protected Array(T[] array, bool asRef = false)
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

        public Array() : this(Array.Empty<T>(), true)
        {
        }

        public Array(params T[] elements) : this(elements, false)
        {
        }

        public Array(int length, Func<T> element)
        {
            Length = length;

            internalArray = new T[Length];

            for (int i = 0; i < Length; i++)
            {
                internalArray[i] = element();
            }
        }

        public static Array<T> FromRef(params T[] array)
        {
            return new Array<T>(array, true);
        }

        public T[] ToMutable()
        {
            var array = new T[Length];

            Array.Copy(internalArray, 0, array, 0, Length);

            return array;
        }

        public ShapedArray<T> ToShape(Shape shape)
        {
            return ShapedArray<T>.FromRef(shape, internalArray);
        }
    }
}