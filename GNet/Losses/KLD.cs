using static System.Math;

namespace GNet.Losses
{
    public class KLD : ILoss
    {
        public double Compute(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => T * Log(T / (O + double.Epsilon))).Avarage();
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -T / (O + double.Epsilon));
        }
    }
}