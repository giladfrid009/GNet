using static System.Math;

namespace GNet.Optimizers
{
    public class AdaGradWindow : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Rho { get; }

        private double epochLr;

        public AdaGradWindow(double learningRate = 0.01, double rho = 0.95, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Rho = rho;
            Decay = decay ?? new Decays.None();
        }

        public void UpdateParams(int epoch)
        {
            epochLr = Decay.Compute(LearningRate, epoch);
        }

        public double Optimize(IOptimizable O)
        {
            O.Cache1 = Rho * O.Cache1 + (1.0 - Rho) * O.Gradient * O.Gradient;
            return -epochLr * O.Gradient / Sqrt(O.Cache1 + double.Epsilon);
        }
    }
}