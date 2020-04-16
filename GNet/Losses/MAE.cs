using static System.Math;

namespace GNet.Losses
{
    public class MAE : ILoss
    {
        public double Compute(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs(T - O)).Avarage();
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => (O - T) / Abs(O - T + double.Epsilon));
        }
    }
}