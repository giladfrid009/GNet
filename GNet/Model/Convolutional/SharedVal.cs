using System;

namespace GNet.Model.Convolutional
{
    [Serializable]
    public class SharedVal<T> : ICloneable<SharedVal<T>> where T : struct
    {
        public SharedVal(T value)
        {
            Value = value;
        }

        public SharedVal() : this(default)
        {
        }

        public T Value { get; set; }

        public SharedVal<T> Clone()
        {
            return new SharedVal<T>(Value);
        }
    }
}
