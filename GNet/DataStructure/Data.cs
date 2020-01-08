using System;

namespace GNet
{
    public readonly struct Data : IEquatable<Data>
    {
        public ShapedArrayImmutable<double> Inputs { get; }
        public ShapedArrayImmutable<double> Outputs { get; }

        public Data(ShapedArrayImmutable<double> inputs, ShapedArrayImmutable<double> outputs)
        {
            Inputs = inputs;
            Outputs = outputs;
        }

        public bool Equals(Data other)
        {
            if (other == null)
            {
                return false;
            }

            return (Inputs, Outputs) == (other.Inputs, other.Outputs);
        }

        public override bool Equals(object? obj)
        {
            return (obj is Data data) && Equals(data);
        }

        public static bool operator ==(Data left, Data right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Data left, Data right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return Inputs.GetHashCode() + Outputs.GetHashCode();
        }
    }
}
