using System;

namespace GNet.Utils.Convolutional
{
    public static class ConvValidator
    {
        public static void CheckParams(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings)
        {
            if (inputShape.Rank != kernelShape.Rank)
            {
                throw new RankException(nameof(kernelShape));
            }

            if (inputShape.Rank != strides.Length)
            {
                throw new RankException(nameof(strides));
            }

            if (inputShape.Rank != paddings.Length)
            {
                throw new RankException(nameof(paddings));
            }

            for (int i = 0; i < strides.Length; i++)
            {
                if (strides[i] < 1)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(strides)} [{i}] is out of range.");
                }

                if (paddings[i] < 0)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(paddings)} [{i}] is out of range.");
                }

                if (inputShape.Dimensions[i] < kernelShape.Dimensions[i])
                {
                    throw new ArgumentOutOfRangeException($"{nameof(kernelShape)} {nameof(kernelShape.Dimensions)} [{i}] is out of range.");
                }

                if ((inputShape.Dimensions[i] + 2 * paddings[i] - kernelShape.Dimensions[i]) % strides[i] != 0)
                {
                    throw new RankException($"Convolution rank [{i}] params are invalid.");
                }
            }
        }
    }
}
