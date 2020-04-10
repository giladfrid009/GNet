using System;

namespace GNet.OutTransformers.Losses
{
    public class BinaryRoundLoss : BinaryRound, ILoss
    {
        public BinaryRoundLoss(double bound = 0.5) : base(bound)
        {
        }

        public double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(Transform(outputs), (T, O) => T == O ? 0.0 : 1.0).Avarage();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            throw new NotSupportedException();
        }
    }
}