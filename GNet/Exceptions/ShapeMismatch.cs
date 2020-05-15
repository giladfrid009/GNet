using System;

namespace GNet
{
    [Serializable]
    public class ShapeMismatchException : Exception
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