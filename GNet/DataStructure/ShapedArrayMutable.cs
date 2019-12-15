using System;

namespace GNet
{
    [Serializable]
    public class ShapedArrayMutable<T> : ShapedArray<T>, ICloneable<ShapedArrayMutable<T>>
    {
        public ShapedArrayMutable(Shape shape) : base(shape)
        {

        }

        public ShapedArrayMutable(Shape shape, Array array) : base(shape, array)
        {

        }

        public ShapedArrayMutable(Shape shape, params T[] array) : base(shape, (Array)array)
        {

        }

        public new T this[params int[] indices]
        {
            get => array[Shape.FlattenIndices(indices)];
            set => array[Shape.FlattenIndices(indices)] = value;
        }

        public new T this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public new ShapedArrayMutable<T> Clone()
        {
            return new ShapedArrayMutable<T>(Shape, array);
        }
    }
}
