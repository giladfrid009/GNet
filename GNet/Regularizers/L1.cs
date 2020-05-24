using static System.Math;

namespace GNet.Regularizers
{
    public class L1 : IRegularizer
    {
        public double Lambda { get; }

        public L1(double lambda = 0.01)
        {
            Lambda = lambda;
        }

        public double Evaluate(double X)
        {
            return Lambda * Abs(X);
        }

        public double Derivative(double X)
        {
            return Lambda * Sign(X);
        }
    }
}
