using System;

namespace GNet.Containers.NumOps
{
    [Serializable]
    public class FloatOps : INumOps<float>
    {
        public float MinValue { get; } = float.MinValue;
        public float MaxValue { get; } = float.MaxValue;

        public float Add(float left, float right) => left + right;
        public float Sub(float left, float right) => left - right;
        public float Mul(float left, float right) => left * right;
        public float Div(float left, float right) => left / right;

        public float Min(float left, float right) => left < right ? left : right;
        public float Max(float left, float right) => left > right ? left : right;

        public bool Equals(float left, float right) => left == right;

        public TOther To<TOther>(float value) where TOther : unmanaged
        {
            return (TOther)(object)value;
        }

        public float From<TOther>(TOther value) where TOther : unmanaged
        {
            return (float)(object)value;
        }
    }
}
