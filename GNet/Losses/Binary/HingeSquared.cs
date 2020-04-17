using static System.Math;

namespace GNet.Losses.Binary
{
    public class HingeSquared : ILoss
    {
        public double Margin { get; }

        public HingeSquared(double margin = 1.0)
        {
            Margin = margin;
        }

        public double Compute(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Max(0.0, Margin - T * O) * Max(0.0, Margin - T * O)).Avarage();
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => T * O < Margin ? -2.0 * T * (Margin - T * O) : 0.0);
        }
    }
}