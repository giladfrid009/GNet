using static System.Math;

namespace GNet.Optimizers
{
    public class AdaGrad : IOptimizer
    {
        public IDecay Decay { get; }
        public double LearningRate { get; }
        
        private double epochLr;

        public AdaGrad(double learningRate = 0.01, IDecay? decay = null)
        {
            LearningRate = learningRate;
            Decay = decay ?? new Decays.None();
        }

        public void UpdateParams(int epoch)
        {
            epochLr = Decay.Compute(LearningRate, epoch);
        }

        public double Optimize(IOptimizable O)
        {
            O.Cache1 += O.Gradient * O.Gradient;
            return -epochLr * O.Gradient / (Sqrt(O.Cache1) + double.Epsilon);
        }
    }
}