using static System.Math;

namespace GNet.Losses
{
    public class LogCosh : ILoss
    {
        public double Compute(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Log(Cosh(T - O))).Avarage();
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Tanh(O - T));
        }
    }
}