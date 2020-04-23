using static System.Math;

namespace GNet.Losses.Regression
{
    public class MAPE : ILoss
    {
        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs((T - O) / (T + double.Epsilon))).Avarage();
        }

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => O * (T - O) / (Pow(T, 3.0) * Abs(1.0 - O / (T + double.Epsilon))));
        }
    }
}