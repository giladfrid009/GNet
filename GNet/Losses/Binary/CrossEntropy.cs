using static System.Math;

namespace GNet.Losses.Binary
{
    public class CrossEntropy : ILoss
    {
        public double Compute(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -T * Log(O + double.Epsilon) - (1.0 - T) * Log(1.0 - O + double.Epsilon)).Avarage();
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => (T - O) / (O * O - O + double.Epsilon));
        }
    }
}