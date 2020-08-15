﻿using System;

namespace GNet.Containers.NumOps
{
    [Serializable]
    public class DoubleOps : NumOps<double>
    {
        public override double MinValue { get; } = double.MinValue;
        public override double MaxValue { get; } = double.MaxValue;

        public override double Add(double left, double right) => left + right;
        public override double Sub(double left, double right) => left - right;
        public override double Mul(double left, double right) => left * right;
        public override double Div(double left, double right) => left / right;

        public override bool Equals(double left, double right) => left == right;
        public override bool Smaller(double left, double right) => left < right;
        public override bool Greater(double left, double right) => left > right;
    }
}
