using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using static System.Math;

namespace GNet.Losses
{
    public class CosineProximity : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            double tProd = targets.Accumulate(1.0, (R, X) => R * X);
            double oProd = outputs.Accumulate(1.0, (R, X) => R * X);
            double tSumSqr = targets.Accumulate(1.0, (R, X) => R + X * X);
            double oSumSqr = outputs.Accumulate(1.0, (R, X) => R + X * X);

            return -tProd * oProd / (Sqrt(tSumSqr) * Sqrt(oSumSqr));
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            double tProd = targets.Accumulate(1.0, (R, X) => R * X);
            double oProd = outputs.Accumulate(1.0, (R, X) => R * X);
            double tSumSqr = targets.Accumulate(1.0, (R, X) => R + X * X);
            double oSumSqr = outputs.Accumulate(1.0, (R, X) => R + X * X);

            return outputs.Select(O => -tProd * (oProd / O) * Pow(oSumSqr - O * O, 2.0) / (Abs(tSumSqr) * Pow(oSumSqr, 1.5)));
        }

        public ILoss Clone()
        {
            return new CosineProximity();
        }
    }
}
