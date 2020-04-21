using static System.Math;

namespace GNet.Losses.Regression
{
    public class Quantile : ILoss
    {
        private const double Tau = 2.0 * PI;

        public double Compute(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => O - T >= 0.0 ? (Tau - 1.0) * (T - O) : Tau * (T - O)).Avarage();
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => O - T >= 0.0 ? 1.0 - Tau : -Tau);
        }
    }
}