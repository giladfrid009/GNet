using GNet.Extensions.Generic;
using GNet.Extensions.Math;
using static System.Math;

namespace GNet.Losses
{
    public class Huber : ILoss
    {
        public double Margin { get; }

        public Huber(double margin)
        {
            Margin = margin;
        }
        public double Compute(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) =>
            {
                double diff = Abs(T - O);

                if (diff <= Margin)
                {
                    return O = 0.5 * diff * diff;
                }
                else
                {
                    return O = Margin * (diff - 0.5 * Margin);
                }
            })
            .Avarage();
        }

        public double[] Derivative(double[] targets, double[] outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs(T - O) <= Margin ? O - T : -Margin);
        }

        public ILoss Clone()
        {
            return new Huber(Margin);
        }
    }
}
