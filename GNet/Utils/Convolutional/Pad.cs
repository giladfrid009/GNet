using System;

namespace GNet.Utils.Convolutional
{
    public static class Pad
    {
        public static Shape Shape(Shape shape, ImmutableArray<int> paddings)
        {
            return new Shape(shape.Dimensions.Select((D, i) => D + 2 * paddings[i]));
        }

        public static ImmutableShapedArray<T> ShapedArray<T>(ImmutableShapedArray<T> array, ImmutableArray<int> paddings, Func<T> padVal)
        {
            Shape paddedShape = Pad.Shape(array.Shape, paddings);

            T[] internalArray = new T[paddedShape.Volume];

            IndexGen.ByStart(array.Shape, paddings).ForEach((idx, i) => internalArray[paddedShape.FlattenIndices(idx)] = array[i]);

            for (int i = 0; i < internalArray.Length; i++)
            {
                internalArray[i] ??= padVal();
            }

            return GNet.ImmutableShapedArray<T>.FromRef(paddedShape, internalArray);
        }
    }
}
