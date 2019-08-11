using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using static System.Math;

namespace GNet.Losses
{
    public class NegativeLogLiklihood : ILoss
    {
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -Log(O)).Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => -1.0 / O);
        }

        public ILoss Clone()
        {
            return new NegativeLogLiklihood();
        }
    }
}
