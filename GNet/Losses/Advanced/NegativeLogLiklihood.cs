using static System.Math;

namespace GNet.Losses.Advanced
{
    public class NegativeLogLiklihood : ILoss
    {
        public double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -Log(O + double.Epsilon)).Avarage();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -1.0 / (O + double.Epsilon));
        }
    }
}