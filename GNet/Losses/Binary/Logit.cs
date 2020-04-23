using static System.Math;

namespace GNet.Losses.Regression
{
    public class Logit : ILoss
    {
        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Log(1.0 + Exp(-T * O))).Avarage();
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -1.0 / (1.0 + Exp(T * O)));
        }
    }
}