using static System.Math;

namespace GNet.Losses
{
    public class Poisson : ILoss
    {
        public double Compute(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => O - T * Log(O + double.Epsilon)).Avarage();
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => 1.0 - T / (O + double.Epsilon));
        }
    }
}