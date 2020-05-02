using static System.Math;

namespace GNet.Losses.Binary
{
    public class HingeSquared : ILoss
    {
        public double Margin { get; }

        public HingeSquared(double margin = 1.0)
        {
            Margin = margin;
        }

        public double Evaluate(double T, double O)
        {
            return T * O < Margin ? Pow(Margin - T * O, 2.0) : 1.0;
        }

        public double Derivative(double T, double O)
        {
            return T * O < Margin ? -2.0 * T * (Margin - T * O) : 0.0;
        }
    }
}