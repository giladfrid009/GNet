using static System.Math;

namespace GNet.Optimizers
{
    public class Adam : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Beta1 { get; }
        public double Beta2 { get; }
        public double Epsilon { get; }

        private double epochLr;
        private double corrDiv1;
        private double corrDiv2;

        public Adam(double learningRate = 0.001, double beta1 = 0.9, double beta2 = 0.999, double epsilon = 1e-8, IDecay? decay = null)
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
            corrDiv1 = 1.0 - Pow(Beta1, epoch + 1.0);
            corrDiv2 = 1.0 - Pow(Beta2, epoch + 1.0);
        }

        public double Optimize(IOptimizable O)
        {
            O.Cache1 = Beta1 * O.Cache1 + (1.0 - Beta1) * O.Gradient;
            O.Cache2 = Beta2 * O.Cache2 + (1.0 - Beta2) * O.Gradient * O.Gradient;
            double corr1 = O.Cache1 / corrDiv1;
            double corr2 = O.Cache2 / corrDiv2;
            return -epochLr * corr1 / (Sqrt(corr2) + Epsilon);
        }
    }
}