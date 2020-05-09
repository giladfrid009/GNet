using static System.Math;

namespace GNet.Losses.Regression
{
    public class MAPE : ILoss
    {
        public double Evaluate(double T, double O)
        {
            return Abs((T - O) / T);
        }

        public double Derivative(double T, double O)
        {
            return O * Sign(1.0 - O / T) / (T * T);
        }
    }
}