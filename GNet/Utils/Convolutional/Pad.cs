using System;

namespace GNet.Utils.Convolutional
{
    public static class Pad
    {
        public static Shape Shape(Shape shape, ArrayImmutable<int> paddings)
        {
            return new Shape(shape.Dimensions.Select((D, i) => D + 2 * paddings[i]));
        }

        public static ShapedArrayImmutable<T> ShapedArray<T>(ShapedArrayImmutable<T> array, ArrayImmutable<int> paddings, Func<T> padVal)
        {
            Shape paddedShape = Pad.Shape(array.Shape, paddings);

            T[] internalArray = new T[paddedShape.Volume];

            IndexGen.ByStart(array.Shape, paddings).ForEach((idx, i) => internalArray[paddedShape.FlattenIndices(idx)] = array[i]);

            for (int i = 0; i < internalArray.Length; i++)
            {
                internalArray[i] ??= padVal();
            }

            return ShapedArrayImmutable<T>.FromRef(paddedShape, internalArray);
        }
    }
}
