﻿using GNet.Extensions.IArray;
using GNet.Extensions.ShapedArray;
using static System.Math;

namespace GNet.Losses
{
    public class NegativeLogLiklihood : ILoss
    {
        public double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -Log(O)).Avarage();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -1.0 / O);
        }

        public ILoss Clone()
        {
            return new NegativeLogLiklihood();
        }
    }
}
