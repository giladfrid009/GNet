using static System.Math;

namespace GNet.Losses.Binary
{
    public class Hinge : ILoss
    {
        public double Margin { get; }

        public Hinge(double margin = 1.0)
        {
            Margin = margin;
        }

        public double Evaluate(double T, double O)
        {
            return Max(0.0, Margin - T * O);
        }

        public double Derivative(double T, double O)
        {
            return T * O < Margin ? -T : 0.0;
        }
    }
}