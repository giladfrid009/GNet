using GNet.Extensions.IArray;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GNet
{
    [Serializable]
    public struct Shape : IEquatable<Shape>
    {
        public ArrayImmutable<int> Dimensions { get; }
        public int NumDimentions => Dimensions.Length;
        public int Volume { get; }

        public Shape(params int[] dimensions)
        {
            for (int i = 0; i < dimensions.Length; i++)
            {
                if (dimensions[i] < 0)
                {
                    throw new ArgumentOutOfRangeException($"Dimensions {i} is out of range.");
                }
            }

            Dimensions = new ArrayImmutable<int>(dimensions);

            Volume = Dimensions.Accumulate(1, (R, X) => R * X);
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

        public bool Equals([AllowNull] Shape other)
        {
            if (other == null)
            {
                return false;
            }

            return Dimensions == other.Dimensions;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj);
        }

        public static bool operator ==(Shape left, Shape right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Shape left, Shape right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return Dimensions.GetHashCode() + Volume * 17;
        }
    }
}
