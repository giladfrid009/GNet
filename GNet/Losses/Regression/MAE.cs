using static System.Math;

namespace GNet.Losses.Regression
{
    public class MAE : ILoss
    {
        public double Compute(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs(T - O)).Avarage();
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => (O - T) / Abs(O - T + double.Epsilon));
        }
    }
}