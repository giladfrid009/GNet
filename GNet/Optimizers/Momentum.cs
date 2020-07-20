namespace GNet.Optimizers
{
    public class Momentum : IOptimizer
    {
        public IDecay? Decay { get; }
        public double LearningRate { get; }
        public double MomentumValue { get; }

        private double epochLr;

        public Momentum(double learningRate = 0.01, double momentum = 0.9, IDecay? decay = null)
        {
            LearningRate = learningRate;
            MomentumValue = momentum;
            Decay = decay;
        }

        public void UpdateEpoch(int epoch)
        {
            epochLr = Decay?.Compute(LearningRate, epoch) ?? LearningRate;
        }

        public double Optimize(TrainableObj O)
        {
            O.Cache1 = -epochLr * O.Gradient + MomentumValue * O.Cache1;
            return O.Cache1;
        }
    }
}