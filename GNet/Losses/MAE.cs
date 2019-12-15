using GNet.Extensions.IArray;
using GNet.Extensions.ShapedArray;
using static System.Math;

namespace GNet.Losses
{
    /// <summary>
    /// Mean Absolute Error
    /// </summary>
    public class MAE : ILoss
    {
        public double Compute(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs(T - O)).Avarage();
        }

        public ShapedReadOnlyArray<double> Derivative(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => T > O ? 1.0 : -1.0);
        }

        public ILoss Clone()
        {
            return new MAE();
        }
    }
}
