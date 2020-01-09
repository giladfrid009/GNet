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
            if(Inputs != other.Inputs)
            {
                return false;
            }

            if(Outputs != other.Outputs)
            {
                return false;
            }

            return true;
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
            return (Inputs, Outputs).GetHashCode();
        }
    }
}
