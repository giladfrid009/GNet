﻿using System;

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