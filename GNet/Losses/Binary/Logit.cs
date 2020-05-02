using static System.Math;

namespace GNet.Losses.Regression
{
    public class Logit : ILoss
    {
        public double Evaluate(double T, double O)
        {
            return Log(1.0 + Exp(-T * O));
        }

        public double Derivative(double T, double O)
        {
            return -1.0 / (1.0 + Exp(T * O));
        }
    }
}