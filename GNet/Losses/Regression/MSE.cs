namespace GNet.Losses.Regression
{
    public class MSE : ILoss
    {
        public double Evaluate(double T, double O)
        {
            return (T - O) * (T - O);
        }

        public double Derivative(double T, double O)
        {
            return 2.0 * (O - T);
        }
    }
}