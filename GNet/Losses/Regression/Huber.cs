using static System.Math;

namespace GNet.Losses.Regression
{
    public class Huber : ILoss
    {
        public double Margin { get; }

        public Huber(double margin)
        {
            Margin = margin;
        }

        public double Evaluate(ImmutableArray<double> targets, ImmutableArray<double> outputs)
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

        public ImmutableArray<double> Derivative(ImmutableArray<double> targets, ImmutableArray<double> outputs)
        {
            return targets.Combine(outputs, (T, O) => Abs(T - O) <= Margin ? O - T : Margin * Sign(O - T));
        }
    }
}