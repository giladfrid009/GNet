﻿using static System.Math;

namespace GNet.Losses
{
    public class Huber : ILoss
    {
        public double Margin { get; }

        public Huber(double margin)
        {
            Margin = margin;
        }

        public double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) =>
            {
                double diff = Abs(T - O);

                if (diff <= Margin)
                {
                    return 0.5 * diff * diff;
                }
                else
                {
                    return Margin * (diff - 0.5 * Margin);
                }
            })
            .Avarage();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs(T - O) <= Margin ? O - T : -Margin);
        }
    }
}