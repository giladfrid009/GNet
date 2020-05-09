using static System.Math;

namespace GNet.Losses.Categorical
{
    public class KLD : ILoss
    {
        public double Evaluate(double T, double O)
        {
            return T * Log(T / O);
        }

        public double Derivative(double T, double O)
        {
            return -T / O;
        }
    }
}