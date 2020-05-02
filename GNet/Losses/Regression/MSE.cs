namespace GNet.Losses.Regression
{
    public class MSE : ILoss
    {
        public double Evaluate(double T, double O)
        {
            return 0.5 * (T - O) * (T - O);
        }

        public double Derivative(double T, double O)
        {
            return O - T;
        }
    }
}