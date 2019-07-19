using System;

// todo: maybe only layers need to have shapes? and data shouldn't?
namespace GNet
{
    public struct Shape : IEquatable<Shape>
    {
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }
        public int Features { get; }
        public int TotalLength { get; }

        public Shape(int width, int height, int depth)
        {
            if (depth < 1 || height < 1 || width < 1)
                throw new ArgumentOutOfRangeException("Depth, height and width must all be positive integers.");

            Width = width;
            Height = height;
            Depth = depth;
            Features = 3;

            TotalLength = Width * Height * Depth;
        }

        public Shape(int width, int height) : this(width, height, 1)
        {
            Features = 2;
        }

        public Shape(int width) : this(width, 1, 1)
        {
            Features = 1;
        }

        public int[] IndexToShape(int index)
        {
            if (index < 0 || index >= TotalLength)
                throw new IndexOutOfRangeException();

            if (Features == 1)
                return new int[] { index };

            if (Features == 2)
            {
                return new int[] { index / Height, index % Height };
            }

            if (Features == 3)
            {
                return new int[] { index / Depth, index % Depth / Height, index % Depth % Height };
            }

            throw new Exception("Unsupported number of features.");
        }

        // todo: maybe reverse arguments?
        public int IndexToFlat(int depth, int height, int width)
        {
            if (depth < 0 || height < 0 || width < 0)
                throw new IndexOutOfRangeException();

            int index = depth * Depth + height * Height + width;

            if (index >= TotalLength)
                throw new IndexOutOfRangeException();

            return index;
        }

        public bool Equals(Shape shape)
        {
            if (shape.TotalLength != TotalLength || shape.Width != Width || shape.Height != Height || shape.Depth != Depth)
                return false;

            return true;
        }

        public override bool Equals(object obj) => (obj is Shape shape) && Equals(shape);

        public override int GetHashCode() => (Width, Height, Depth, Features, TotalLength).GetHashCode();

        public static bool operator ==(Shape shape1, Shape shape2) => shape1.Equals(shape2);

        public static bool operator !=(Shape shape1, Shape shape2) => !shape1.Equals(shape2);
    }
}
