using System;

namespace GNet.Containers.NumOps
{
    [Serializable]
    public class IntOps : NumOps<int>
    {
        public override int MinValue { get; } = int.MinValue;
        public override int MaxValue { get; } = int.MaxValue;

        public override int Add(int left, int right) => left + right;
        public override int Sub(int left, int right) => left - right;
        public override int Mul(int left, int right) => left * right;
        public override int Div(int left, int right) => left / right;

        public override bool Equals(int left, int right) => left == right;
        public override bool Smaller(int left, int right) => left < right;
        public override bool Bigger(int left, int right) => left > right;
    }
}
