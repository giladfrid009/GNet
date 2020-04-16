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

        public double Compute(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) =>
            {
                double diff = Abs(T - O);

                if (diff <= Margin)
                {
                    return 0.5 * diff * diff;
                }
                else
                {
                    return Margin * (diff - 0.5 * Margin);
                }
            })
            .Avarage();
        }

        public ImmutableShapedArray<double> Derivative(ImmutableShapedArray<double> targets, ImmutableShapedArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs(T - O) <= Margin ? O - T : -Margin);
        }
    }
}