using static System.Math;

namespace GNet.Losses.Regression
{
    public class MAE : ILoss
    {
        public double Evaluate(double T, double O)
        {
            return Abs(T - O);
        }

        public double Derivative(double T, double O)
        {
            return Sign(O - T);
        }
    }
}