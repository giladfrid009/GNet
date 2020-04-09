using System;
using System.Collections.Generic;

namespace GNet.Utils.Convolutional
{
    public static class IndexGen
    {
        public static ArrayImmutable<int[]> Generate(Shape shape, ArrayImmutable<int> start, ArrayImmutable<int> strides, Shape kernel)
        {
            if (strides.Length != shape.Rank)
            {
                throw new ArgumentOutOfRangeException("Strides dimensions length mismatch.");
            }

            if (start.Length != shape.Rank)
            {
                throw new ArgumentOutOfRangeException("Start dimensions length mismatch.");
            }

            if (kernel.Rank != shape.Rank)
            {
                throw new ArgumentOutOfRangeException("Kernel dimensions length mismatch.");
            }

            for (int i = 0; i < shape.Rank; i++)
            {
                if (start[i] < 0 || strides[i] < 1 || kernel.Dimensions[i] > shape.Dimensions[i])
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            int lastIndex = shape.Rank - 1;

            var indices = new List<int[]>();

            PopulateRecursive(new int[shape.Rank], 0);

            return new ArrayImmutable<int[]>(indices);

            void PopulateRecursive(int[] current, int dim)
            {
                int bound = start[dim] + shape.Dimensions[dim] - (kernel.Dimensions[dim] - 1);

                if (dim == lastIndex)
                {
                    for (int i = start[dim]; i < bound; i += strides[dim])
                    {
                        current[dim] = i;
                        int[] clone = new int[current.Length];
                        Array.Copy(current, 0, clone, 0, current.Length);
                        indices.Add(clone);
                    }
                }
                else
                {
                    for (int i = start[dim]; i < bound; i += strides[dim])
                    {
                        current[dim] = i;
                        PopulateRecursive(current, dim + 1);
                    }
                }

                current[dim] = 0;
            }
        }

        public static ArrayImmutable<int[]> ByStrides(Shape shape, ArrayImmutable<int> strides, Shape kernel)
        {
            return Generate(shape, new ArrayImmutable<int>(shape.Rank, () => 0), strides, kernel);
        }

        public static ArrayImmutable<int[]> ByStart(Shape shape, ArrayImmutable<int> start)
        {
            return Generate(shape, start, new ArrayImmutable<int>(shape.Rank, () => 1), new Shape(new ArrayImmutable<int>(shape.Rank, () => 1)));
        }
    }
}