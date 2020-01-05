
using GNet.Extensions.IArray;
using GNet.Extensions.IShapedArray;
using static System.Math;

namespace GNet.Losses
{
    public class CosineProximity : ILoss
    {
        public double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            double tProd = targets.Accumulate(1.0, (R, X) => R * X);
            double oProd = outputs.Accumulate(1.0, (R, X) => R * X);
            double tSumSqr = targets.Sum(X => X * X);
            double oSumSqr = outputs.Sum(X => X * X);

            return -tProd * oProd / (Sqrt(tSumSqr) * Sqrt(oSumSqr));
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            double tProd = targets.Accumulate(1.0, (R, X) => R * X);
            double oProd = outputs.Accumulate(1.0, (R, X) => R * X);
            double tSumSqr = targets.Sum(X => X * X);
            double oSumSqr = outputs.Sum(X => X * X);

            return outputs.Select(O => -tProd * (oProd / O) * Pow(oSumSqr - O * O, 2.0) / (Abs(tSumSqr) * Pow(oSumSqr, 1.5)));
        }

        public ILoss Clone()
        {
            return new CosineProximity();
        }
    }
}
