using GNet.Extensions.IArray;
using GNet.Extensions.ShapedArray;
using static System.Math;

namespace GNet.Losses
{
    public class BinaryCrossEntropy : ILoss
    {
        public double Compute(ShapedArray<double> targets, ShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -T * Log(O) - (1.0 - T) * Log(1.0 - O)).Avarage();
        }

        public ShapedArray<double> Derivative(ShapedArray<double> targets, ShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => (T - O) / (O * O - O));
        }

        public ILoss Clone()
        {
            return new BinaryCrossEntropy();
        }
    }
}
