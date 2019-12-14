﻿using GNet.Extensions.Array.Generic;
using GNet.Extensions.Array.Math;
using GNet.Extensions.ShapedArray.Generic;
using GNet.Extensions.ShapedArray.Math;
using System;

namespace GNet
{
    [Serializable]
    public class Shape : IEquatable<Shape>, ICloneable<Shape>
    {
        public int[] Dimensions { get; }

        public int NumDimentions { get; }

        public Shape(params int[] dimensions)
        {
            dimensions.ForEach((X, i) =>
            {
                if (X < 0)
                {
                    throw new ArgumentOutOfRangeException($"Dimension {i} is out of range.");
                }
            });

            Dimensions = dimensions.Select(X => X);
            NumDimentions = dimensions.Length;
        }

        public int Length()
        {
            return Dimensions.Accumulate(1, (R, X) => R * X);
        }

        public int FlattenIndices(params int[] indices)
        {
            indices.ForEach((X, i) =>
            {
                if (indices[i] > Dimensions[i] - 1 || indices[i] < 0)
                {
                    throw new IndexOutOfRangeException($"Indices {i} is out of range.");
                }
            });

            int index = 0;
            return indices.Accumulate(0, (R, X) => R * Dimensions[index++] + X);
        }

        public bool Equals(Shape? other)
        {
            if (other == null)
            {
                return false;
            }

            if (NumDimentions != other.NumDimentions)
            {
                return false;
            }

            for (int i = 0; i < NumDimentions; i++)
            {
                if (Dimensions[i] != other.Dimensions[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Equals((Shape)obj);
        }

        public override int GetHashCode()
        {
            return NumDimentions + Dimensions.Accumulate(1, (R, X) => unchecked(R * 314159 + X));
        }

        public Shape Clone()
        {
            return new Shape(Dimensions);
        }
    }
}
