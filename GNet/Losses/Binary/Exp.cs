using static System.Math;

namespace GNet.Losses.Binary
{
    public class Exp : ILoss
    {
        public double Evaluate(double T, double O)
        {
            return Exp(-T * O);
        }

        public double Derivative(double T, double O)
        {
            return -Exp(-T * O);
        }
    }
}