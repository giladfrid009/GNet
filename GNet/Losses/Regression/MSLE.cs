using static System.Math;

namespace GNet.Losses.Regression
{
    public class MSLE : ILoss
    {
        public double Compute(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Pow(Log((T + 1.0) / (O + 1.0)), 2.0)).Avarage();
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -2.0 * Log((T + 1.0) / (O + 1.0)) / (O + 1.0));
        }
    }
}