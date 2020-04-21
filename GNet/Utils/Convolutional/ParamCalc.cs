using System;

namespace GNet.Utils.Convolutional
{
    public static class ParamCalc
    {
        public static Shape CalcOutputShape(Shape inputShape, Shape kernelShape, ImmutableArray<int> strides, ImmutableArray<int> paddings)
        {
            int length = inputShape.Rank;

            if (kernelShape.Rank != length)
            {
                throw new RankException(nameof(kernelShape));
            }

            if (strides.Length != length)
            {
                throw new RankException(nameof(strides));
            }

            if (paddings.Length != length)
            {
                throw new RankException(nameof(paddings));
            }

            int[] outDims = new int[length];

            for (int i = 0; i < length; i++)
            {
                if (strides[i] < 1)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(strides)} [{i}] is out of range.");
                }

                if (paddings[i] < 0)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(paddings)} [{i}] is out of range.");
                }

                if (inputShape.Dims[i] < kernelShape.Dims[i])
                {
                    throw new ArgumentOutOfRangeException($"{nameof(kernelShape)} {nameof(kernelShape.Dims)} [{i}] is out of range.");
                }

                int currentDim = (inputShape.Dims[i] + 2 * paddings[i] - kernelShape.Dims[i]) % strides[i];

                if(currentDim % strides[i] != 0)
                {
                    throw new RankException($"Convolution {nameof(inputShape.Rank)} [{i}] params are invalid.");
                }

                outDims[i] = currentDim;
            }

            return new Shape(outDims);
        }

        public static ImmutableArray<int> CalcPaddings(Shape inputShape, Shape outputShape, Shape kernelShape, ImmutableArray<int> strides)
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

                int doublePad = strides[i] * (outputShape.Dims[i] - 1) - inputShape.Dims[i] + kernelShape.Dims[i];

                if (doublePad < 0 || doublePad % 2 != 0)
                {
                    throw new RankException($"Convolution {nameof(inputShape.Rank)} [{i}] params are invalid.");
                }

                paddings[i] = doublePad / 2;
            }

            return ImmutableArray<int>.FromRef(paddings);
        }
    }
}
