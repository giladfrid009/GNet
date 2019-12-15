using System;

namespace GNet
{
    [Serializable]
    public class ShapedArray<T> : ShapedReadOnlyArray<T>, ICloneable<ShapedArray<T>>
    {
        public ShapedArray(Shape shape) : base(shape)
        {

        }

        public ShapedArray(Shape shape, Array array) : base(shape, array)
        {

        }

        public ShapedArray(Shape shape, params T[] array) : base(shape, (Array)array)
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

        public new ShapedArray<T> Clone()
        {
            return new ShapedArray<T>(Shape, array);
        }
    }
}
