using System;
using System.Collections.Generic;

namespace GNet.Layers
{
    public static class ConvHelpers
    {
        public static ArrayImmutable<int[]> GenerateIndices(Shape shape, ArrayImmutable<int> start, ArrayImmutable<int> strides, Shape kernel)
        {
            if (strides.Length != shape.NumDimentions)
            {
                throw new ArgumentOutOfRangeException("Strides dimensions length mismatch.");
            }

            if (start.Length != shape.NumDimentions)
            {
                throw new ArgumentOutOfRangeException("Start dimensions length mismatch.");
            }

            if (kernel.NumDimentions != shape.NumDimentions)
            {
                throw new ArgumentOutOfRangeException("Kernel dimensions length mismatch.");
            }

            for (int i = 0; i < shape.NumDimentions; i++)
            {
                if (start[i] < 0 || strides[i] < 1 || kernel.Dimensions[i] > shape.Dimensions[i])
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            int lastIndex = shape.NumDimentions - 1;

            var indices = new List<int[]>();

            PopulateRecursive(new int[shape.NumDimentions], 0);

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

        public static ArrayImmutable<int[]> IndicesByStrides(Shape shape, ArrayImmutable<int> strides, Shape kernel)
        {
            return GenerateIndices(shape, new ArrayImmutable<int>(shape.NumDimentions, () => 0), strides, kernel);
        }

        public static ArrayImmutable<int[]> IndicesByStart(Shape shape, ArrayImmutable<int> start)
        {
            return GenerateIndices(shape, start, new ArrayImmutable<int>(shape.NumDimentions, () => 1), new Shape(new ArrayImmutable<int>(shape.NumDimentions, () => 1)));
        }
    }
}
