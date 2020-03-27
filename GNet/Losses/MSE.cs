namespace GNet.Losses
{
    /// <summary>
    /// Mean Squared Error
    /// </summary>
    public class MSE : ILoss
    {
        public double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => (T - O) * (T - O)).Avarage();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => 2.0 * (O - T));
        }
    }
}