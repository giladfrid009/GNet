using GNet.Model;
using static System.Math;

namespace GNet.Optimizers
{
    public class AdaDelta : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Rho { get; }

        private double epochLr;

        public AdaDelta(double learningRate = 1.0, double rho = 0.95, IDecay? decay = null)
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
            double delta = Sqrt(O.Cache2 + double.Epsilon) * O.Gradient / Sqrt(O.Cache1 + double.Epsilon);
            O.Cache2 = Rho * O.Cache2 + (1.0 - Rho) * delta * delta;
            return -epochLr * delta;
        }
    }
}