using GNet.Extensions.IArray;
using GNet.Extensions.ShapedArray;

namespace GNet.Losses
{
    /// <summary>
    /// Mean Squared Error
    /// </summary>
    public class MSE : ILoss
    {
        public double Compute(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => (T - O) * (T - O)).Avarage();
        }

        public ShapedReadOnlyArray<double> Derivative(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => 2.0 * (O - T));
        }

        public ILoss Clone()
        {
            return new MSE();
        }
    }
}
