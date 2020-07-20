using static System.Math;

namespace GNet.Optimizers
{
    public class Nadam : IOptimizer
    {
        public IDecay? Decay { get; }
        public double LearningRate { get; }
        public double Beta1 { get; }
        public double Beta2 { get; }
        public double Epsilon { get; }

        private double epochLr;
        private double corrDiv1;
        private double corrDiv2;

        public Nadam(double learningRate = 0.002, double beta1 = 0.9, double beta2 = 0.999, double epsilon = 1e-08, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Beta1 = beta1;
            Beta2 = beta2;
            Epsilon = epsilon;
            Decay = decay;
        }

        public void UpdateEpoch(int epoch)
        {
            epochLr = Decay?.Compute(LearningRate, epoch) ?? LearningRate;
            corrDiv1 = 1.0 - Pow(Beta1, epoch + 1.0);
            corrDiv2 = 1.0 - Pow(Beta2, epoch + 1.0);
        }

        public double Optimize(TrainableObj O)
        {
            O.Cache1 = Beta1 * O.Cache1 + (1.0 - Beta1) * O.Gradient;
            O.Cache2 = Beta2 * O.Cache2 + (1.0 - Beta2) * O.Gradient * O.Gradient;
            double biasCorr1 = O.Cache1 / corrDiv1 + (1.0 - Beta1) * O.Gradient / corrDiv1;
            double biasCorr2 = O.Cache2 / corrDiv2;
            return -epochLr * biasCorr1 / Sqrt(biasCorr2 + Epsilon);
        }
    }
}