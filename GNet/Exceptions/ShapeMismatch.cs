using System;

namespace GNet
{
    internal class ShapeMismatchException : Exception
    {
        public ShapeMismatchException() : base()
        {
        }

        public ShapeMismatchException(string? messege) : base(messege)
        {
        }

        public ShapeMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}