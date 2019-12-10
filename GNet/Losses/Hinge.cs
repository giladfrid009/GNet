﻿using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using static System.Math;

namespace GNet.Losses
{
    public class Hinge : ILoss
    {
        public double Margin { get; }

        public Hinge(double margin = 1.0)
        {
            Margin = margin;
        }
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => Max(0, Margin - T * O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => T * O < Margin ? -T : 0.0);
        }

        public ILoss Clone()
        {
            return new Hinge(Margin);
        }
    }
}