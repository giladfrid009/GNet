using System;
using System.Collections.Generic;

namespace GNet.Utils.Convolutional
{
    public static class IndexGen
    {
        public static ImmutableArray<int[]> Generate(Shape shape, ImmutableArray<int> start, ImmutableArray<int> strides, Shape kernel)
        {
            if (shape.Rank != strides.Length)
            {
                throw new RankException(nameof(strides));
            }

            if (shape.Rank != start.Length)
            {
                throw new RankException(nameof(start));
            }

            if (shape.Rank != kernel.Rank)
            {
                throw new RankException(nameof(kernel));
            }

            for (int i = 0; i < shape.Rank; i++)
            {
                if(start[i] < 0)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(start)} [{i}] is out of range.");
                }

                if (strides[i] < 1)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(strides)} [{i}] is out of range.");
                }

                if (kernel.Dims[i] > shape.Dims[i])
                {
                    throw new ArgumentOutOfRangeException($"{nameof(kernel)} {nameof(kernel.Dims)} [{i}] is out of range.");
                }
            }

            int lastIndex = shape.Rank - 1;

            var indices = new List<int[]>();

            PopulateRecursive(new int[shape.Rank], 0);

            return new ImmutableArray<int[]>(indices.ToArray());

            void PopulateRecursive(int[] current, int dim)
            {
                int bound = start[dim] + shape.Dims[dim] - (kernel.Dims[dim] - 1);

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

        public static ImmutableArray<int[]> ByStrides(Shape shape, ImmutableArray<int> strides, Shape kernel)
        {
            return Generate(shape, new ImmutableArray<int>(shape.Rank, () => 0), strides, kernel);
        }

        public static ImmutableArray<int[]> ByStart(Shape shape, ImmutableArray<int> start)
        {
            return Generate(shape, start, new ImmutableArray<int>(shape.Rank, () => 1), new Shape(new ImmutableArray<int>(shape.Rank, () => 1)));
        }
    }
}