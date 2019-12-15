using GNet.Extensions.IArray;
using GNet.Extensions.ShapedArray;
using static System.Math;

namespace GNet.Losses
{
    /// <summary>
    /// Mean Squared Logarithmic Error
    /// </summary>
    public class MSLE : ILoss
    {
        public double Compute(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => (Log(T + 1.0) - Log(O + 1.0)) * (Log(T + 1.0) - Log(O + 1.0))).Avarage();
        }

        public ShapedArray<double> Derivative(ShapedReadOnlyArray<double> targets, ShapedReadOnlyArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => -2.0 * (Log(T + 1.0) - Log(O + 1.0)) / (O + 1.0));
        }

        public ILoss Clone()
        {
            return new MSLE();
        }
    }
}
