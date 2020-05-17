using static System.Math;

namespace GNet.Optimizers
{
    public class RMSProp : IOptimizer
    {
        public IDecay? Decay { get; }
        public double LearningRate { get; }
        public double Rho { get; }
        public double Epsilon { get; }

        private double epochLr;

        public RMSProp(double learningRate = 0.001, double rho = 0.9, double epsilon = 1e-8, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Rho = rho;
            Epsilon = epsilon;
            Decay = decay;
        }

        public void UpdateParams(int epoch)
        {
            epochLr = Decay?.Compute(LearningRate, epoch) ?? LearningRate;
        }

        public double Optimize(IOptimizable O)
        {
            O.Cache1 = Rho * O.Cache1 + (1.0 - Rho) * O.Gradient * O.Gradient;
            return -epochLr * O.Gradient / Sqrt(O.Cache1 + Epsilon);
        }
    }
}