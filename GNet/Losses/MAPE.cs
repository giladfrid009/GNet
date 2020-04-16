using static System.Math;

namespace GNet.Losses
{
    public class MAPE : ILoss
    {
        public double Compute(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs((T - O) / (T + double.Epsilon))).Avarage();
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => O * (T - O) / (Pow(T, 3.0) * Abs(1.0 - O / (T + double.Epsilon))));
        }
    }
}