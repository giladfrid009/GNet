using static System.Math;

namespace GNet.Losses
{
    /// <summary>
    /// Mean Absolute Error
    /// </summary>
    public class MAE : ILoss
    {
        public double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs(T - O)).Avarage();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => T > O ? 1.0 : -1.0);
        }

        public ILoss Clone()
        {
            return new MAE();
        }
    }
}