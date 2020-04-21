using System;

namespace GNet.Utils.Convolutional
{
    public static class Padder
    {
        public static Shape PadShape(Shape shape, ImmutableArray<int> paddings)
        {
            if (shape.Rank != paddings.Length)
            {
                throw new RankException(nameof(shape));
            }

            return new Shape(shape.Dims.Select((D, i) => D + 2 * paddings[i]));
        }

        public static ImmutableShapedArray<T> PadArray<T>(ImmutableShapedArray<T> array, ImmutableArray<int> paddings, Func<T> padVal)
        {
            if(array.Shape.Rank != paddings.Length)
            {
                throw new RankException(nameof(array));
            }

            Shape paddedShape = PadShape(array.Shape, paddings);

            T[] internalArray = new T[paddedShape.Volume];

            IndexGen.ByStart(array.Shape, paddings).ForEach((idx, i) => internalArray[paddedShape.FlattenIndices(idx)] = array[i]);

            for (int i = 0; i < internalArray.Length; i++)
            {
                internalArray[i] ??= padVal();
            }

            return ImmutableShapedArray<T>.FromRef(paddedShape, internalArray);
        }
    }
}
