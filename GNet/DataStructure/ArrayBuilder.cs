using System;
using System.Collections.Generic;

namespace GNet
{
    public class ArrayBuilder<T> : IArray<T>
    {
        public int Length => internalList.Count;

        public T this[int i]
        {
            get => internalList[i];
            set => internalList[i] = value;
        }

        protected readonly List<T> internalList;

        public ArrayBuilder()
        {
            internalList = new List<T>();
        }

        public ArrayBuilder(int length)
        {
            internalList = new List<T>(length);
        }

        public ArrayBuilder(params T[] array) : this(array.Length)
        {
            for (int i = 0; i < array.Length; i++)
            {
                internalList[i] = array[i];
            }
        }

        public ArrayBuilder(IList<T> list) : this(list.Count)
        {
            int length = list.Count;

            for (int i = 0; i < length; i++)
            {
                internalList[i] = list[i];
            }
        }

        public ArrayBuilder(IEnumerable<T> enumerable)
        {
            internalList = new List<T>(enumerable);
        }

        public ArrayBuilder(int length, Func<T> element) : this(length)
        {
            for (int i = 0; i < Length; i++)
            {
                internalList[i] = element();
            }
        }

        public void Add(T item)
        {
            internalList.Add(item);
        }

        public ArrayImmutable<T> ToImmutable()
        {
            T[] array = internalList.ToArray();

            internalList.Clear();

            return new ArrayImmutable<T>(in array);
        }

        public ShapedArrayImmutable<T> ToShapedImmutable(Shape shape)
        {
            T[] array = internalList.ToArray();

            internalList.Clear();

            return new ShapedArrayImmutable<T>(shape, in array);
        }
    }
}
