using System;

namespace GNet.DataStructure.NumOps
{
    [Serializable]
    public class IntOps : INumOps<int>
    {
        public int MinValue { get; } = int.MinValue;
        public int MaxValue { get; } = int.MaxValue;

        public int Add(int left, int right) => left + right;
        public int Sub(int left, int right) => left - right;
        public int Mul(int left, int right) => left * right;
        public int Div(int left, int right) => left / right;

        public int Min(int left, int right) => left < right ? left : right;
        public int Max(int left, int right) => left > right ? left : right;

        public bool Equals(int left, int right) => left == right;

        public TOther To<TOther>(int value) where TOther : unmanaged
        {
            return (TOther)(object)value;
        }

        public int From<TOther>(TOther value) where TOther : unmanaged
        {
            return (int)(object)value;
        }
    }
}
