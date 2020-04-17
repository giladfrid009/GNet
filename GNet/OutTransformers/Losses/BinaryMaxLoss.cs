﻿using System;

namespace GNet.OutTransformers.Losses
{
    public class BinaryMaxLoss : BinaryMax, ILoss
    {
        public double Compute(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(Transform(outputs), (T, O) => T == O ? 0.0 : 1.0).Avarage();
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            throw new NotSupportedException();
        }
    }
}