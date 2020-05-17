using static System.Math;

namespace GNet.Optimizers
{
    public class AdaMax : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Beta1 { get; }
        public double Beta2 { get; }
        public double Epsilon { get; }

        private double epochLr;
        private double corrDiv;

        public AdaMax(double learningRate = 0.002, double beta1 = 0.9, double beta2 = 0.999, double epsilon = 1e-8, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Beta1 = beta1;
            Beta2 = beta2;
            Epsilon = epsilon;
            Decay = decay ?? new Decays.None();
        }

        public void UpdateParams(int epoch)
        {
            epochLr = Decay.Compute(LearningRate, epoch);
            corrDiv = 1.0 - Pow(Beta1, epoch + 1.0);
        }

        public double Optimize(IOptimizable O)
        {
            O.Cache1 = Beta1 * O.Cache1 + (1.0 - Beta1) * O.Gradient;
            O.Cache2 = Max(Beta2 * O.Cache2, Abs(O.Gradient));
            double biasCorr = O.Cache1 / corrDiv;
            return -epochLr * biasCorr / (O.Cache2 + Epsilon);
        }
    }
}