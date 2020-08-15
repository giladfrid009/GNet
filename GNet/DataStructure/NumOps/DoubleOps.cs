using System;

namespace GNet.DataStructure.NumOps
{
    [Serializable]
    public class DoubleOps : INumOps<double>
    {
        public double MinValue { get; } = double.MinValue;
        public double MaxValue { get; } = double.MaxValue;

        public double Add(double left, double right) => left + right;
        public double Sub(double left, double right) => left - right;
        public double Mul(double left, double right) => left * right;
        public double Div(double left, double right) => left / right;

        public double Min(double left, double right) => left < right ? left : right;
        public double Max(double left, double right) => left > right ? left : right;

        public bool Equals(double left, double right) => left == right;

        public TOther To<TOther>(double value) where TOther : unmanaged
        {
            return (TOther)(object)value;
        }

        public double From<TOther>(TOther value) where TOther : unmanaged
        {
            return (double)(object)value;
        }
    }
}
