namespace GNet.Regularizers
{
    public class L2 : IRegularizer
    {
        public double Lambda { get; }

        public L2(double lambda = 0.01)
        {
            Lambda = lambda;
        }

        public double Evaluate(double X)
        {
            return Lambda * X * X;
        }

        public double Derivative(double X)
        {
            return 2.0 * Lambda * X;
        }
    }
}
