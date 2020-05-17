using static System.Math;

namespace GNet.Optimizers
{
    public class AdaGrad : IOptimizer
    {
        public IDecay? Decay { get; }
        public double LearningRate { get; }
        public double Epsilon { get; }

        private double epochLr;

        public AdaGrad(double learningRate = 0.01, double epsilon = 1e-8, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Epsilon = epsilon;
            Decay = decay;
        }

        public void UpdateParams(int epoch)
        {
            epochLr = Decay?.Compute(LearningRate, epoch) ?? LearningRate;
        }

        public double Optimize(TrainableObj O)
        {
            O.Cache1 += O.Gradient * O.Gradient;
            return -epochLr * O.Gradient / Sqrt(O.Cache1 + Epsilon);
        }
    }
}