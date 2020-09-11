using System;

namespace GNet
{
    [Serializable]
    public class SharedVal<T> where T : unmanaged
    {
        public T Value { get; set; }

        public SharedVal(T value)
        {
            Value = value;
        }

        public SharedVal() : this(default)
        {
        }
    }
}