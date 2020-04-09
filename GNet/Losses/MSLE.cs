using static System.Math;

namespace GNet.Losses
{
    public class MSLE : ILoss
    {
        public double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Pow(Log((T + 1.0) / (O + 1.0)), 2.0)).Avarage();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -2.0 * Log((T + 1.0) / (O + 1.0)) / (O + 1.0));
        }
    }
}