using GNet.Extensions.IArray;
using GNet.Extensions.IShapedArray;
using static System.Math;

namespace GNet.Losses
{
    public class Poisson : ILoss
    {
        public double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => O - T * Log(O)).Avarage();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => 1.0 - T / O);
        }

        public ILoss Clone()
        {
            return new Poisson();
        }
    }
}
