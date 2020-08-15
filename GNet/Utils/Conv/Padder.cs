using System;

namespace GNet.Utils.Conv
{
    public static class Padder
    {
        public static VArray<int> CalcPadding(Shape inputShape, Shape outputShape, Shape kernelShape, VArray<int> strides, bool padChannels)
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

            return VArray<int>.FromRef(paddings);
        }

        public static ShapedArray<T> PadArray<T>(ShapedArray<T> array, VArray<int> paddings, Func<T> padVal)
        {
            if (array.Shape.Rank != paddings.Length)
            {
                throw new RankException(nameof(array));
            }

            Shape paddedShape = array.Shape.Pad(paddings);

            var internalArray = new T[paddedShape.Volume];

            IndexGen.ByStart(array.Shape, paddings).ForEach((idx, i) => internalArray[paddedShape.FlattenIndices(idx)] = array[i]);

            for (int i = 0; i < internalArray.Length; i++)
            {
                internalArray[i] ??= padVal();
            }

            return ShapedArray<T>.FromRef(paddedShape, internalArray);
        }
    }
}