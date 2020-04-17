﻿using static System.Math;

namespace GNet.Losses.Regression
{
    public class CosineProximity : ILoss
    {
        public double Compute(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            double tProd = targets.Accumulate(1.0, (R, X) => R * X);
            double oProd = outputs.Accumulate(1.0, (R, X) => R * X);
            double tSumSqr = targets.Sum(X => X * X);
            double oSumSqr = outputs.Sum(X => X * X);

            return -tProd * oProd / (Sqrt(tSumSqr + double.Epsilon) * Sqrt(oSumSqr + double.Epsilon));
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            double tProd = targets.Accumulate(1.0, (R, X) => R * X);
            double oProd = outputs.Accumulate(1.0, (R, X) => R * X);
            double tSumSqr = targets.Sum(X => X * X);
            double oSumSqr = outputs.Sum(X => X * X);

            return outputs.Select(O => -tProd * (oProd / (O + double.Epsilon)) * Pow(oSumSqr - O * O, 2.0) / (Abs(tSumSqr) * Pow(oSumSqr, 1.5)));
        }
    }
}