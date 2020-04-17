using static System.Math;

namespace GNet.Losses.Regression
{
    public class Poisson : ILoss
    {
        public double Compute(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => O - T * Log(O + double.Epsilon)).Avarage();
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => 1.0 - T / (O + double.Epsilon));
        }
    }
}