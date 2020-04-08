﻿using static System.Math;

namespace GNet.Losses
{
    public class MAPE : ILoss
    {
        public double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs(T - O) / (T + double.Epsilon)).Avarage();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => (O - T) / (T * Abs(O - T) + double.Epsilon));
        }
    }
}
