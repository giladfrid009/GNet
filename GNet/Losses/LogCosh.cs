using GNet.Extensions.IArray;
using GNet.Extensions.IShapedArray;
using static System.Math;

namespace GNet.Losses
{
    public class LogCosh : ILoss
    {
        public double Compute(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Log(Cosh(T - O))).Avarage();
        }

        public ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> targets, ShapedArrayImmutable<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Tanh(O - T));
        }

        public ILoss Clone()
        {
            return new LogCosh();
        }
    }
}
