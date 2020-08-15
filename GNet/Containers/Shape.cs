using System;

namespace GNet
{
    [Serializable]
    public class Shape : IEquatable<Shape>
    {
        public static Shape Empty { get; } = new Shape(0);

        public VArray<int> Dims { get; }
        public int Rank { get; }
        public int Volume { get; }

        public Shape(VArray<int> dims)
        {
            int length = dims.Length;

            if (length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dims));
            }

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

        public Shape(params int[] dimensions) : this(new VArray<int>(dimensions))
        {
        }

        public static bool operator !=(Shape? left, Shape? right)
        {
            return !(left == right);
        }

        public static bool operator ==(Shape? left, Shape? right)
        {
            return left?.Equals(right) ?? ReferenceEquals(left, right);
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

        public Shape Pad(VArray<int> paddings)
        {
            return new Shape(Dims.Select(paddings, (D, P) => D + 2 * P, (D, P) => D + 2 * P));
        }

        public bool Equals(Shape? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (Rank != other.Rank)
            {
                return false;
            }

            return Dims.Equals(other.Dims);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Shape);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Dims, Volume * 17);
        }
    }
}