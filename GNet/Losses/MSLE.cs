using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using static System.Math;

namespace GNet.Losses
{
    /// <summary>
    /// Mean Squared Logarithmic Error
    /// </summary>
    public class MSLE : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => (Log(T + 1.0) - Log(O + 1.0)) * (Log(T + 1.0) - Log(O + 1.0))).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -2.0 * (Log(T + 1.0) - Log(O + 1.0)) / (O + 1.0));
        }

        public ILoss Clone()
        {
            return new MSLE();
        }
    }
}
