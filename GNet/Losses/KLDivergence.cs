using GNet.Extensions.IArray;
using GNet.Extensions.ShapedArray;
using static System.Math;

namespace GNet.Losses
{
    public class KLDivergence : ILoss
    {
        public double Compute(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => T * Log(T / O)).Avarage();
        }

        public ShapedReadOnlyArray<double> Derivative(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -T / O);
        }

        public ILoss Clone()
        {
            return new KLDivergence();
        }
    }
}
