using System;

namespace GNet
{
    [Serializable]
    public class Shape : IEquatable<Shape>
    {
        public ArrayImmutable<int> Dimensions { get; }
        public int Rank { get; }
        public int Volume { get; }

        public Shape(ArrayImmutable<int> dimensions)
        {
            int length = dimensions.Length;

            for (int i = 0; i < length; i++)
            {
                if (dimensions[i] < 0)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(dimensions)} [{i}] is out of range.");
                }
            }

            Dimensions = dimensions;
            Rank = dimensions.Length;
            Volume = (int)dimensions.Accumulate(1, (R, X) => R * X);
        }

        public Shape(params int[] dimensions) : this(new ArrayImmutable<int>(dimensions))
        {
        }

        public int FlattenIndices(params int[] indices)
        {
            int length = indices.Length;

            if (length != Rank)
            {
                throw new RankException(nameof(indices));
            }

            for (int i = 0; i < length; i++)
            {
                if (indices[i] < 0 || indices[i] > Dimensions[i] - 1)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(indices)} [{i}] is out of range.");
                }
            }

            int flat = 0;

            for (int i = 0; i < Rank; i++)
            {
                flat = flat * Dimensions[i] + indices[i];
            }

            return flat;
        }

        public static bool operator !=(Shape left, Shape right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(Shape left, Shape right)
        {
            return left.Equals(right);
        }

        public bool Equals(Shape other)
        {
            if(Rank != other.Rank)
            {
                return false;
            }

            for (int i = 0; i < Rank; i++)
            {
                if(Dimensions[i] != other.Dimensions[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            return (obj is Shape shape) && Equals(shape);
        }

        public override int GetHashCode()
        {
            return Dimensions.GetHashCode() + Volume * 17;
        }
    }
}