using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using static System.Math;

namespace GNet.Losses
{
    public class BinaryCrossEntropy : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -T * Log(O) - (1.0 - T) * Log(1.0 - O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => (T - O) / (O * O - O));
        }

        public ILoss Clone()
        {
            return new BinaryCrossEntropy();
        }
    }
}
