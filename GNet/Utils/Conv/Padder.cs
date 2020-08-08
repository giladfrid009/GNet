using System;

namespace GNet.Utils.Conv
{
    public static class Padder
    {
        public static ImmutableArray<int> CalcPadding(in Shape inputShape, in Shape outputShape, in Shape kernelShape, in ImmutableArray<int> strides, bool padChannels)
        {
            int length = inputShape.Rank;

            if (outputShape.Rank != length)
            {
                throw new RankException(nameof(outputShape));
            }

            if (kernelShape.Rank != length)
            {
                throw new RankException(nameof(kernelShape));
            }

            if (strides.Length != length)
            {
                throw new RankException(nameof(strides));
            }

            int[] paddings = new int[length];

            for (int i = 0; i < length; i++)
            {
                if (strides[i] < 1)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(strides)} [{i}] is out of range.");
                }

                if (inputShape.Dims[i] < kernelShape.Dims[i])
                {
                    throw new ArgumentOutOfRangeException($"{nameof(kernelShape)} {nameof(kernelShape.Dims)} [{i}] is out of range.");
                }

                if (padChannels == false && i == 0)
                {
                    continue;
                }

                int doublePad = strides[i] * (outputShape.Dims[i] - 1) - inputShape.Dims[i] + kernelShape.Dims[i];

                if (doublePad < 0 || doublePad % 2 != 0)
                {
                    throw new RankException($"Convolution {nameof(inputShape.Rank)} [{i}] params are invalid.");
                }

                paddings[i] = doublePad / 2;
            }

            return ImmutableArray<int>.FromRef(paddings);
        }

        public static Shape PadShape(in Shape shape, ImmutableArray<int> paddings)
        {
            if (shape.Rank != paddings.Length)
            {
                throw new RankException(nameof(shape));
            }

            return new Shape(shape.Dims.Select((D, i) => D + 2 * paddings[i]));
        }

        public static ImmutableShapedArray<T> PadShapedArray<T>(in ImmutableShapedArray<T> array, in ImmutableArray<int> paddings, Func<T> padVal)
        {
            if (array.Shape.Rank != paddings.Length)
            {
                throw new RankException(nameof(array));
            }

            Shape paddedShape = PadShape(array.Shape, paddings);

            var internalArray = new T[paddedShape.Volume];

            var indices = IndexGen.ByStart(array.Shape, paddings);

            for (int i = 0; i < indices.Length; i++)
            {
                internalArray[paddedShape.FlattenIndices(indices[i])] = array[i];
            }

            for (int i = 0; i < internalArray.Length; i++)
            {
                internalArray[i] ??= padVal();
            }

            return ImmutableShapedArray<T>.FromRef(paddedShape, internalArray);
        }
    }
}