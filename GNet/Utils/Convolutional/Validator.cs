using System;

namespace GNet.Utils.Convolutional
{
    public static class Validator
    {
        public static void CheckParams(Shape inputShape, Shape kernelShape, ArrayImmutable<int> strides, ArrayImmutable<int> paddings)
        {
            if (inputShape.Rank != kernelShape.Rank)
            {
                throw new ArgumentException("KernelShape dimensions count mismatch.");
            }

            if (inputShape.Rank != strides.Length)
            {
                throw new ArgumentException("Strides dimensions count mismatch.");
            }

            if (inputShape.Rank != paddings.Length)
            {
                throw new ArgumentException("Paddings dimensions count mismatch.");
            }

            for (int i = 0; i < strides.Length; i++)
            {
                if (strides[i] < 1)
                {
                    throw new ArgumentOutOfRangeException($"Strides [{i}] is out of range.");
                }

                if (paddings[i] < 0)
                {
                    throw new ArgumentOutOfRangeException($"Paddings [{i}] is out of range.");
                }

                if (inputShape.Dimensions[i] < kernelShape.Dimensions[i])
                {
                    throw new ArgumentOutOfRangeException($"KernelShape dimension [{i}] is out of range.");
                }

                if ((inputShape.Dimensions[i] + 2 * paddings[i] - kernelShape.Dimensions[i]) % strides[i] != 0)
                {
                    throw new ArgumentOutOfRangeException($"Dimension [{i}] params are invalid.");
                }
            }
        }
    }
}
