using System;

namespace GNet.Containers.ValArray.Operations
{
    [Serializable]
    public class FloatOps : VOps<float>
    {
        public override float MinValue { get; } = float.MinValue;
        public override float MaxValue { get; } = float.MaxValue;
               
        public override float Add(float left, float right) => left + right;
        public override float Sub(float left, float right) => left - right;
        public override float Mul(float left, float right) => left * right;
        public override float Div(float left, float right) => left / right;

        public override bool Equals(float left, float right) => left == right;
        public override bool Smaller(float left, float right) => left < right;
        public override bool Bigger(float left, float right) => left > right;
    }
}
