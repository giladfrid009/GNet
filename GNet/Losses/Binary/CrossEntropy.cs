using static System.Math;

namespace GNet.Losses.Binary
{
    public class CrossEntropy : ILoss
    {
        public double Evaluate(double T, double O)
        {
            return -T * Log(O + double.Epsilon) - (1.0 - T) * Log(1.0 - O + double.Epsilon);
        }

        public double Derivative(double T, double O)
        {
            return (T - O) / (O * O - O + double.Epsilon);
        }
    }
}