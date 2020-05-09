using static System.Math;

namespace GNet.Optimizers
{
    public class AdaGrad : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        public double Epsilon { get; }

        private double epochLr;

        //todo: fix, doesn't work
        public AdaGrad(double learningRate = 0.01, double epsilon = 1e-8, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Epsilon = epsilon;
            Decay = decay ?? new Decays.None();
        }

        public void UpdateParams(int epoch)
        {
            epochLr = Decay.Compute(LearningRate, epoch);
        }

        public double Optimize(IOptimizable O)
        {
            O.Cache1 += O.Gradient * O.Gradient;
            return -epochLr * O.Gradient / (Sqrt(O.Cache1) + Epsilon);
        }
    }
}