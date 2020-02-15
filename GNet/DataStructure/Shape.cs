using System;
using System.Collections.Generic;

namespace GNet
{
    [Serializable]
    public readonly struct Shape : IEquatable<Shape>
    {
        public ArrayImmutable<int> Dimensions { get; }
        public int NumDimentions => Dimensions.Length;
        public int Volume { get; }

        public Shape(ArrayImmutable<int> dimensions)
        {
            for (int i = 0; i < dimensions.Length; i++)
            {
                if (dimensions[i] < 0)
                {
                    throw new ArgumentOutOfRangeException($"Dimensions {i} is out of range.");
                }
            }

            Dimensions = dimensions;

            Volume = (int)dimensions.Accumulate(1, (R, X) => R * X);
        }

        public Shape(params int[] dimensions) : this(new ArrayImmutable<int>(dimensions))
        {

        }

        public int FlattenIndices(params int[] indices)
        {
            if (indices.Length > NumDimentions)
            {
                throw new ArgumentOutOfRangeException("Indices length is out of range.");
            }

            for (int i = 0; i < NumDimentions; i++)
            {
                if (indices[i] < 0 || indices[i] > Dimensions[i] - 1)
                {
                    throw new IndexOutOfRangeException($"Indices {i} is out of range.");
                }
            }

            int flatIndex = 0;

            for (int i = 0; i < NumDimentions; i++)
            {
                flatIndex = flatIndex * Dimensions[i] + indices[i];
            }

            return flatIndex;
        }

        public ArrayImmutable<int[]> GetIndices(ArrayImmutable<int> start, ArrayImmutable<int> strides)
        {
            if (strides.Length != NumDimentions || start.Length != NumDimentions)
            {
                throw new ArgumentOutOfRangeException("Strides or Start length mismatch.");
            }

            for (int i = 0; i < NumDimentions; i++)
            {
                if(start[i] < 0 || strides[i] < 1 || start[i] + strides[i] >= Dimensions[i])
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            int lastIndex = NumDimentions - 1;

            var indices = new List<int[]>();

            PopulateRecursive(this, new int[NumDimentions], 0);

            return new ArrayImmutable<int[]>(indices);

            void PopulateRecursive(Shape shape, int[] current, int dim)
            {
                int bound = start[dim] + shape.Dimensions[dim];

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
                        PopulateRecursive(shape, current, dim + 1);
                    }
                }

                current[dim] = 0;
            }
        }

        public ArrayImmutable<int[]> GetIndices()
        {
            return GetIndices(new ArrayImmutable<int>(NumDimentions, () => 0), new ArrayImmutable<int>(NumDimentions, () => 1));
        }

        public ArrayImmutable<int[]> GetIndicesFrom(ArrayImmutable<int> start)
        {
            return GetIndices(start, new ArrayImmutable<int>(NumDimentions, () => 1));
        }     

        public ArrayImmutable<int[]> GetIndicesByStrides(ArrayImmutable<int> strides)
        {
            return GetIndices(new ArrayImmutable<int>(NumDimentions, () => 0), strides);
        }

        public ArrayImmutable<int[]> GetIndices(int[] start, int[] strides)
        {
            return GetIndices(new ArrayImmutable<int>(start), new ArrayImmutable<int>(strides));
        }

        public ArrayImmutable<int[]> GetIndicesFrom(int[] start)
        {
            return GetIndicesFrom(new ArrayImmutable<int>(start));
        }

        public ArrayImmutable<int[]> GetIndicesByStrides(int[] strides)
        {
            return GetIndicesByStrides(new ArrayImmutable<int>(strides));
        }

        public bool Equals(Shape other)
        {
            return Dimensions == other.Dimensions;
        }

        public override bool Equals(object? obj)
        {
            return (obj is Shape shape) && Equals(shape);
        }

        public static bool operator ==(Shape left, Shape right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Shape left, Shape right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return Dimensions.GetHashCode() + Volume * 17;
        }
    }
}
