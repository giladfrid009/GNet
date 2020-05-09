using static System.Math;

namespace GNet.Losses.Regression
{
    public class Poisson : ILoss
    {
        public double Evaluate(double T, double O)
        {
            return O - T * Log(O);
        }

        public double Derivative(double T, double O)
        {
            return 1.0 - T / O;
        }
    }
}