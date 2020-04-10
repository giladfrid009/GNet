namespace GNet.OutTransformers.Losses
{
    public class BinaryMaxLoss : BinaryMax, ILoss
    {
        public double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(Transform(outputs), (T, O) => T == O ? 0.0 : 1.0).Avarage();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(Transform(outputs), (T, O) => T - O);
        }
    }
}