using GNet.Extensions.Generic;
using GNet.Extensions.Math;

namespace GNet.Losses
{
    /// <summary>
    /// Mean Squared Error
    /// </summary>
    public class MSE : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => (T - O) * (T - O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => 2.0 * (O - T));
        }

        public ILoss Clone()
        {
            return new MSE();
        }
    }
}
