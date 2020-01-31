using static System.Math;

namespace GNet.Losses
{
    public class Hinge : ILoss
    {
        public double Margin { get; }

        public Hinge(double margin = 1.0)
        {
            Margin = margin;
        }
        public double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Max(0, Margin - T * O)).Avarage();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => T * O < Margin ? -T : 0.0);
        }

        public ILoss Clone()
        {
            return new Hinge(Margin);
        }
    }
}
