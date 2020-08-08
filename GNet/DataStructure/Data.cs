using System;

namespace GNet
{
    [Serializable]
    public readonly struct Data : IEquatable<Data>
    {
        public ImmutableShapedArray<double> Inputs { get; }
        public ImmutableShapedArray<double> Targets { get; }
        public Shape InputShape { get; }
        public Shape TargetShape { get; }

        public Data(in ImmutableShapedArray<double> inputs, in ImmutableShapedArray<double> targets)
        {
            Inputs = inputs;
            Targets = targets;
            InputShape = inputs.Shape;
            TargetShape = targets.Shape;
        }

        public static bool operator !=(in Data left, in Data right)
        {
            return !(left == right);
        }

        public static bool operator ==(in Data left, in Data right)
        {
            return left.Equals(right);
        }

        public bool Equals(Data other)
        {
            return (Inputs, Targets) == (other.Inputs, other.Targets);
        }

        public override bool Equals(object? obj)
        {
            return (obj is Data data) && Equals(data);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Inputs, Targets);
        }
    }
}