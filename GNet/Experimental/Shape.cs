using System;

// todo: each layer shoud have a shape? each data should have a shape?
namespace GNet.Experimental
{
    class Shape
    {
        public int Width { get; }
        public int Height { get; }
        public int Depth { get; }
        public int Features { get; }
        public int TotalLength { get; }

        public Shape(int width, int height, int depth)
        {
            if (depth < 1 || height < 1 || width < 1)
                throw new ArgumentOutOfRangeException("depth, height and width must all be positive integers");

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
    }
}
