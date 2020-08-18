using NCollections;
using System;
using System.Collections.Generic;

namespace GNet.Utils.Conv
{
    public static class IndexGen
    {
        public static Array<int[]> Generate(Shape shape, NArray<int> start, NArray<int> strides, Shape kernel)
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

            if (start < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(start)} must be >= 0.");
            }

            if (strides < 1)
            {
                throw new ArgumentOutOfRangeException($"{nameof(strides)} must be >= 1.");
            }

            if (kernel.Dims < shape.Dims)
            {
                throw new ArgumentOutOfRangeException($"{nameof(kernel)} can't be bigger than {nameof(shape)}.");
            }

            int lastIndex = shape.Rank - 1;

            var indices = new List<int[]>();

            PopulateRecursive(new int[shape.Rank], 0);

            return Array<int[]>.FromRef(indices.ToArray());

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

        public static Array<int[]> ByStrides(Shape shape, NArray<int> strides, Shape kernel)
        {
            return Generate(shape, new NArray<int>(shape.Rank, () => 0), strides, kernel);
        }

        public static Array<int[]> ByStart(Shape shape, NArray<int> start)
        {
            return Generate(shape, start, new NArray<int>(shape.Rank, () => 1), new Shape(new NArray<int>(shape.Rank, () => 1)));
        }
    }
}