using static System.Math;

namespace GNet.Losses.Regression
{
    public class LogCosh : ILoss
    {
        public double Evaluate(double T, double O)
        {
            return Log(Cosh(T - O));
        }

        public double Derivative(double T, double O)
        {
            return Tanh(O - T);
        }
    }
}