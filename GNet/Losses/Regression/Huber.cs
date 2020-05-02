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

        public double Evaluate(double T, double O)
        {
            double diff = Abs(T - O);

            if (diff <= Margin)
            {
                return 0.5 * diff * diff;
            }

            return Margin * (diff - 0.5 * Margin);
        }

        public double Derivative(double T, double O)
        {
            return Abs(T - O) <= Margin ? O - T : Margin * Sign(O - T);
        }
    }
}