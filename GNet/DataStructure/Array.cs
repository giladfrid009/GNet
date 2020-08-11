using System;

namespace GNet
{
    [Serializable]
    public class Array<T> : BaseArray<T>
    {
        protected override T[] InternalArray { get; }

        protected Array(T[] array, bool asRef = false) : base(array.Length)
        {
            if (asRef)
            {
                InternalArray = array;
            }
            else
            {
                InternalArray = new T[Length];

                Array.Copy(array, 0, InternalArray, 0, Length);
            }
        }

        public Array() : this(Array.Empty<T>(), true)
        {
        }

        public Array(params T[] elements) : this(elements, false)
        {
        }

        public Array(int length, Func<T> element) : base(length)
        {
            InternalArray = new T[Length];

            for (int i = 0; i < Length; i++)
            {
                InternalArray[i] = element();
            }
        }

        public static Array<T> FromRef(params T[] array)
        {
            return new Array<T>(array, true);
        }

        public Array<TRes> Select<TRes>(Func<T, TRes> selector)
        {
            return Select((X, i) => selector(X));
        }

        public Array<TRes> Select<TRes>(Func<T, int, TRes> selector)
        {
            var selected = new TRes[Length];

            for (int i = 0; i < Length; i++)
            {
                selected[i] = selector(InternalArray[i], i);
            }

            return Array<TRes>.FromRef(selected);
        }       

        public T[] ToMutable()
        {
            var array = new T[Length];

            Array.Copy(InternalArray, 0, array, 0, Length);

            return array;
        }

        public ShapedArray<T> ToShape(Shape shape)
        {
            return ShapedArray<T>.FromRef(shape, InternalArray);
        }
    }
}