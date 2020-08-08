using System;

namespace GNet
{
    [Serializable]
    public readonly struct Shape : IEquatable<Shape>
    {
        public ImmutableArray<int> Dims { get; }
        public int Rank { get; }
        public int Volume { get; }

        public Shape(in ImmutableArray<int> dims)
        {
            int length = dims.Length;

            for (int i = 0; i < length; i++)
            {
                if (dims[i] < 1)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(dims)} [{i}] is out of range.");
                }
            }

            Dims = dims;
            Rank = length;

            Volume = 1;

            for (int i = 0; i < length; i++)
            {
                Volume *= dims[i];
            }
        }

        public Shape(params int[] dimensions) : this(new ImmutableArray<int>(dimensions))
        {
        }

        public static bool operator !=(in Shape left, in Shape right)
        {
            return !(left == right);
        }

        public static bool operator ==(in Shape left, in Shape right)
        {
            return left.Equals(right);
        }

        public int FlattenIndices(params int[] indices)
        {
            if (indices.Length != Rank)
            {
                throw new RankException(nameof(indices));
            }

            for (int i = 0; i < indices.Length; i++)
            {
                if (indices[i] < 0 || indices[i] > Dims[i] - 1)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(indices)} [{i}] is out of range.");
                }
            }

            int flat = 0;

            for (int i = 0; i < Rank; i++)
            {
                flat = flat * Dims[i] + indices[i];
            }

            return flat;
        }

        public bool Equals(Shape other)
        {
            if (Rank != other.Rank)
            {
                return false;
            }

            for (int i = 0; i < Rank; i++)
            {
                if (Dims[i] != other.Dims[i])
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
            return Dims.GetHashCode() + Volume * 17;
        }
    }
}