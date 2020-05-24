using static System.Math;

namespace GNet.Regularizers
{
    public class L1L2 : IRegularizer
    {
        public double Lambda1 { get; }
        public double Lambda2 { get; }

        public L1L2(double lambda1 = 0.01, double lambda2 = 0.01)
        {
            Lambda1 = lambda1;
            Lambda2 = lambda2;
        }

        public double Evaluate(double X)
        {
            return Lambda1 * Abs(X) + Lambda2 * X * X;
        }

        public double Derivative(double X)
        {
            return Lambda1 * Sign(X) + 2.0 * Lambda2 * X;
        }
    }
}
