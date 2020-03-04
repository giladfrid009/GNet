using System;

namespace GNet.Initializers
{
    [Serializable]
    public class Const : IInitializer
    {
        public double Value { get; }

        public Const(double value)
        {
            Value = value;
        }

        public double Initialize(int nIn, int nOut)
        {
            return Value;
        }

        public IInitializer Clone()
        {
            return new Const(Value);
        }
    }
}