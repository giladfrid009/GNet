using GNet.Extensions.Array.Generic;
using GNet.Extensions.Array.Math;
using GNet.Extensions.ShapedArray.Generic;
using GNet.Extensions.ShapedArray.Math;

namespace GNet.Losses
{
    /// <summary>
    /// Mean Squared Error
    /// </summary>
    public class MSE : ILoss
    {
        public double Compute(ShapedArray<double> targets, ShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => (T - O) * (T - O)).Avarage();
        }

        public ShapedArray<double> Derivative(ShapedArray<double> targets, ShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => 2.0 * (O - T));
        }

        public ILoss Clone()
        {
            return new MSE();
        }
    }
}
