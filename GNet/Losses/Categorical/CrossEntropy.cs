using static System.Math;

namespace GNet.Losses.Categorical
{
    public class CrossEntropy : ILoss
    {
        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -T * Log(O + double.Epsilon)).Avarage();
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -T / (O + double.Epsilon));
        }
    }
}