using static System.Math;

namespace GNet.Optimizers
{
    public class AdaDelta : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Rho { get; }
        public double Epsilon { get; }

        private double epochLr;

        //todo: fix, doesnt work
        public AdaDelta(double learningRate = 1.0, double rho = 0.95, double epsilon = 1e-08, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Rho = rho;
            Epsilon = epsilon;
            Decay = decay ?? new Decays.None();
        }

        public void UpdateParams(int epoch)
        {
            epochLr = Decay.Compute(LearningRate, epoch);
        }

        public double Optimize(IOptimizable O)
        {
            O.Cache1 = Rho * O.Cache1 + (1.0 - Rho) * O.Gradient * O.Gradient;
            double delta = Sqrt(O.Cache2 + Epsilon) * O.Gradient / Sqrt(O.Cache1 + Epsilon);
            O.Cache2 = Rho * O.Cache2 + (1.0 - Rho) * delta * delta;
            return -epochLr * delta;
        }
    }
}