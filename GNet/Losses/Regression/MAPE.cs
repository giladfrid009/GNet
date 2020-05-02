using static System.Math;

namespace GNet.Losses.Regression
{
    public class MAPE : ILoss
    {
        public double Evaluate(double T, double O)
        {
            return Abs((T - O) / (T + double.Epsilon));
        }

        public double Derivative(double T, double O)
        {
            return O * (T - O) / (Pow(T, 3.0) * Abs(1.0 - O / (T + double.Epsilon)));
        }
    }
}