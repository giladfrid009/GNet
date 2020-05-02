using static System.Math;

namespace GNet.Losses.Regression
{
    public class Quantile : ILoss
    {
        private const double Tau = 2.0 * PI;

        public double Evaluate(double T, double O)
        {
            return O - T >= 0.0 ? (Tau - 1.0) * (T - O) : Tau * (T - O);
        }

        public double Derivative(double T, double O)
        {
            return O - T >= 0.0 ? 1.0 - Tau : -Tau;
        }
    }
}