using static System.Math;

namespace GNet.Losses.Regression
{
    public class MSLE : ILoss
    {
        public double Evaluate(double T, double O)
        {
            return Pow(Log((T + 1.0) / (O + 1.0)), 2.0);
        }

        public double Derivative(double T, double O)
        {
            return -2.0 * Log((T + 1.0) / (O + 1.0)) / (O + 1.0);
        }
    }
}