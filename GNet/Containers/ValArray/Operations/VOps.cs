using System;

namespace GNet.Containers.ValArray
{
    [Serializable]
    public abstract class VOps<T> where T : unmanaged
    {
        public abstract T MinValue { get; }
        public abstract T MaxValue { get; }

        public abstract T Add(T left, T right);
        public abstract T Sub(T left, T right);
        public abstract T Div(T left, T right);
        public abstract T Mul(T left, T right);       

        public abstract bool Equals(T left, T right);
        public abstract bool Smaller(T left, T right);
        public abstract bool Bigger(T left, T right);

        public T Min(T left, T right)
        {
            return Smaller(left, right) ? left : right;
        }

        public T Max(T left, T right)
        {
            return Bigger(left, right) ? left : right;
        }

        public bool SmallerEqual(T left, T right)
        {
            return Smaller(left, right) || Equals(left, right);
        }

        public bool BiggerEqual(T left, T right)
        {
            return Bigger(left, right) || Equals(left, right);
        }

        public TOther To<TOther>(T value) where TOther : unmanaged
        {
            return (TOther)(object)value;
        }

        public T From<TOther>(TOther value) where TOther : unmanaged
        {
            return (T)(object)value;
        }
    }
}
