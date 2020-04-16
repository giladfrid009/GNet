using static System.Math;

namespace GNet.Losses
{
    public class BinaryCrossEntropy : ILoss
    {
        public double Compute(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -T * Log(O + double.Epsilon) - (1.0 - T) * Log(1.0 - O + double.Epsilon)).Avarage();
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => (T - O) / (O * O - O + double.Epsilon));
        }
    }
}